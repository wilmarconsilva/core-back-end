import { useCallback, useEffect, useMemo, useState } from 'react';
import { AuthenticationDependencies } from '../../../app/createAuthenticationDependencies';
import { AccessToken } from '../../../domain/authentication/entities/AccessToken';
import { Session } from '../../../domain/authentication/entities/Session';
import { AuthenticationViewModel } from '../viewModels/AuthenticationViewModel';

export function useAuthenticationViewModel(dependencies: AuthenticationDependencies): AuthenticationViewModel {
  const [apiBaseUrl, setApiBaseUrl] = useState(dependencies.defaultApiBaseUrl);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [feedback, setFeedback] = useState('Informe a URL da API e faça login.');
  const [session, setSession] = useState<Session>({ accessToken: null });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isRestoringSession, setIsRestoringSession] = useState(true);

  const applyAccessToken = useCallback((accessToken: AccessToken) => {
    setSession({ accessToken });
  }, []);

  const clearSession = useCallback(() => {
    setSession({ accessToken: null });
  }, []);

  const execute = useCallback(
    async (action: () => Promise<void>, silent = false) => {
      setIsSubmitting(true);

      try {
        await action();
      } catch (error) {
        clearSession();

        if (!silent) {
          setFeedback(error instanceof Error ? error.message : 'Ocorreu um erro inesperado.');
        }
      } finally {
        setIsSubmitting(false);
        setIsRestoringSession(false);
      }
    },
    [clearSession],
  );

  const login = useCallback(async () => {
    if (!email.trim() || !password.trim()) {
      setFeedback('Informe e-mail e senha para continuar.');
      return;
    }

    await execute(async () => {
      const accessToken = await dependencies.authenticationService.login(apiBaseUrl, {
        email: email.trim(),
        password,
      });

      applyAccessToken(accessToken);
      setFeedback('Login realizado com sucesso.');
    });
  }, [apiBaseUrl, applyAccessToken, dependencies.authenticationService, email, execute, password]);

  const refreshSession = useCallback(async (silent = false) => {
    await execute(async () => {
      const accessToken = await dependencies.authenticationService.refresh(apiBaseUrl);
      applyAccessToken(accessToken);

      if (!silent) {
        setFeedback('Sessão renovada com sucesso.');
      }
    }, silent);
  }, [apiBaseUrl, applyAccessToken, dependencies.authenticationService, execute]);

  const logout = useCallback(async () => {
    await execute(async () => {
      await dependencies.authenticationService.logout(apiBaseUrl);
      clearSession();
      setPassword('');
      setFeedback('Sessão encerrada.');
    });
  }, [apiBaseUrl, clearSession, dependencies.authenticationService, execute]);

  useEffect(() => {
    void (async () => {
      await execute(async () => {
        const accessToken = await dependencies.authenticationService.refresh(apiBaseUrl);
        applyAccessToken(accessToken);
      }, true);
    })();
  }, []);

  const formattedExpiration = useMemo(() => {
    if (session.accessToken === null) {
      return '-';
    }

    return new Date(session.accessToken.expirationDateTime).toLocaleString('pt-BR');
  }, [session.accessToken]);

  const accessTokenPreview = useMemo(() => {
    if (session.accessToken === null) {
      return '-';
    }

    return `${session.accessToken.token.slice(0, 28)}...`;
  }, [session.accessToken]);

  return {
    apiBaseUrl,
    email,
    password,
    feedback,
    isAuthenticated: session.accessToken !== null,
    isSubmitting,
    isRestoringSession,
    accessTokenPreview,
    formattedExpiration,
    setApiBaseUrl,
    setEmail,
    setPassword,
    login,
    refreshSession: () => refreshSession(false),
    logout,
  };
}
