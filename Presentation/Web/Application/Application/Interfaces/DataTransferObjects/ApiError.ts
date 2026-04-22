export interface ApiError {
  title?: string;
  detail?: string;
  status?: number;
  errors?: Record<string, string[]>;
}