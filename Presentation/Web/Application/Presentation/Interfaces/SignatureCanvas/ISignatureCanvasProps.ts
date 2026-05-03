export interface ISignatureCanvasProps {
  onSignatureChange: (signatureBase64: string | null) => void;
  width?: number;
  height?: number;
}
