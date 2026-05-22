import { IDocumentTemplate } from '../../Application/Interfaces/DataTransferObjects/Commercial/Documents/IDocumentTemplate';

export function get(data: IDocumentTemplate): string {
  const observationsBlock = data.observations
    ? `<p><span class="label">Observações:</span> ${escapeHtml(data.observations)}</p>`
    : '';

  return `<!DOCTYPE html>
<html lang="pt-BR">
<head>
  <meta charset="utf-8"/>
  <title>${escapeHtml(data.documentTitle)} ${data.id}</title>
  <style>
    body { font-family: Arial, sans-serif; margin: 40px; color: #333; }
    h1 { border-bottom: 2px solid #333; padding-bottom: 10px; }
    .info p { margin: 4px 0; }
    .totals { margin-top: 20px; text-align: right; }
    .totals p { margin: 4px 0; }
    .label { font-weight: bold; }
  </style>
</head>
<body>
  <h1>${escapeHtml(data.documentTitle)} ${data.id}</h1>
  <div class="info">
    <p><span class="label">Cliente:</span> ${escapeHtml(data.customerName)}</p>
    <p><span class="label">Data:</span> ${escapeHtml(data.creationDateTime)}</p>
    <p><span class="label">Forma de pagamento:</span> ${escapeHtml(data.paymentMethod)}</p>
    ${observationsBlock}
  </div>
  <div class="totals">
    <p><span class="label">Subtotal:</span> ${escapeHtml(data.valueSubtotal)}</p>
    <p><span class="label">Total:</span> ${escapeHtml(data.valueTotal)}</p>
  </div>
</body>
</html>`;
}

function escapeHtml(value: string): string {
  return value
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;');
}
