export function renderTemplate(template: string, data: Record<string, unknown>): string {
  let result = template;

  // Handle {{#each array}}...{{/each}}
  result = result.replace(/\{\{#each\s+(\w+)\}\}([\s\S]*?)\{\{\/each\}\}/g, (_, key, inner) => {
    const array = data[key];
    if (!Array.isArray(array)) return '';
    return array.map((item) => {
      if (typeof item !== 'object' || item === null) return inner.replace(/\{\{this\}\}/g, String(item));
      return inner.replace(/\{\{(\w+)\}\}/g, (_match: string, field: string) => {
        const val = (item as Record<string, unknown>)[field];
        return val !== undefined && val !== null ? String(val) : '';
      });
    }).join('');
  });

  // Handle {{#if field}}...{{/if}}
  result = result.replace(/\{\{#if\s+(\w+)\}\}([\s\S]*?)\{\{\/if\}\}/g, (_, key, inner) => {
    return data[key] ? inner : '';
  });

  // Handle {{#unless field}}...{{/unless}}
  result = result.replace(/\{\{#unless\s+(\w+)\}\}([\s\S]*?)\{\{\/unless\}\}/g, (_, key, inner) => {
    return !data[key] ? inner : '';
  });

  // Handle {{field}} simple substitution
  result = result.replace(/\{\{(\w+)\}\}/g, (_, key) => {
    const val = data[key];
    return val !== undefined && val !== null ? String(val) : '';
  });

  return result;
}
