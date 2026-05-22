import React, { useCallback, useEffect, useRef, useState } from 'react';
import { Platform } from 'react-native';
import { useReactToPrint } from 'react-to-print';

export type DocumentPrintFn = (html: string) => Promise<void>;

export interface UseDocumentPrintResult {
  printHtml: DocumentPrintFn;
  printHost: React.ReactNode;
}

export function UseDocumentPrint(): UseDocumentPrintResult {
  const contentRef = useRef<HTMLDivElement>(null);
  const [html, setHtml] = useState('');
  const resolveRef = useRef<(() => void) | null>(null);

  const handlePrint = useReactToPrint({
    contentRef,
    onAfterPrint: () => {
      setHtml('');
      resolveRef.current?.();
      resolveRef.current = null;
    },
  });

  useEffect(() => {
    if (!html) return;
    handlePrint();
  }, [html, handlePrint]);

  const printHtml = useCallback<DocumentPrintFn>((documentHtml) => {
    if (Platform.OS !== 'web') return Promise.resolve();
    return new Promise((resolve) => {
      resolveRef.current = resolve;
      setHtml(documentHtml);
    });
  }, []);

  const printHost =
    html && Platform.OS === 'web'
      ? React.createElement('div', {
          ref: contentRef,
          style: { display: 'none' },
          dangerouslySetInnerHTML: { __html: html },
        })
      : null;

  return { printHtml, printHost };
}
