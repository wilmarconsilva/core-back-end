import React, { ReactNode, useEffect, useRef } from 'react';
import { Animated } from 'react-native';

interface ScreenTransitionProps {
  /** A unique key for the current screen; change triggers the fade-in animation. */
  screenKey: string;
  children: ReactNode;
}

/**
 * Wraps a screen with a lightweight fade-in animation whenever `screenKey` changes.
 * Uses the native driver for opacity to keep animation off the JS thread.
 */
export function ScreenTransition({ screenKey, children }: ScreenTransitionProps) {
  const opacity = useRef(new Animated.Value(0)).current;

  useEffect(() => {
    opacity.setValue(0);
    Animated.timing(opacity, {
      toValue: 1,
      duration: 220,
      useNativeDriver: true,
    }).start();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [screenKey]);

  return (
    <Animated.View style={{ flex: 1, opacity }}>
      {children}
    </Animated.View>
  );
}
