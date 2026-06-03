import { OperationMode, OperationModeService } from '../Services/Offline/OperationModeService';

export interface UseOperationModeResult {
  mode: OperationMode;
  isOffline: boolean;
}

/**
 * Exposes the operation mode resolved at startup to the presentation layer.
 * The mode is stable for the whole session, so no subscription is required.
 */
export function UseOperationMode(): UseOperationModeResult {
  return {
    mode: OperationModeService.getMode(),
    isOffline: OperationModeService.isOffline(),
  };
}
