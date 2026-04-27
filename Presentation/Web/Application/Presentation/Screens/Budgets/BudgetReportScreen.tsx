import React, { useState } from 'react';
import { ActivityIndicator, Platform, ScrollView, StyleSheet, Text, TouchableOpacity, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { DateField } from '../../Components/DateField';
import { PrimaryButton } from '../../Components/PrimaryButton';
import { BudgetReportService } from '../../../Application/Services/Budgets/BudgetReportService';
import { TemplateService } from '../../../Application/Services/TemplateService';
import { renderTemplate } from '../../../Application/Services/TemplateRendererService';
import { IBudgetReportFilter } from '../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetReportFilter';
import { IBudgetReportListing } from '../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetReportListing';
import { IBudgetReport } from '../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetReport';
import { colors, layoutStyles, typographyStyles } from '../../Styles';

interface BudgetReportScreenProps {
  onGoBack: () => void;
}

const budgetReportService = new BudgetReportService();
const templateService = new TemplateService();

const styles = StyleSheet.create({
  filterRow: { flexDirection: 'row', gap: 8, marginBottom: 12, alignItems: 'flex-end', flexWrap: 'wrap' },
  filterField: { flex: 1, minWidth: 140 },
  actionBtn: { marginTop: 0, paddingHorizontal: 16, paddingVertical: 12 },
  sectionCard: {
    backgroundColor: colors.surface,
    borderRadius: 12,
    padding: 16,
    marginBottom: 16,
    shadowColor: colors.shadow,
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.06,
    shadowRadius: 4,
    elevation: 2,
  },
  summaryBox: {
    backgroundColor: colors.primary,
    borderRadius: 12,
    padding: 20,
    marginBottom: 16,
    alignItems: 'center',
  },
  summaryLabel: { color: colors.textWhite, fontSize: 14, marginBottom: 4 },
  summaryValue: { color: colors.textWhite, fontSize: 28, fontWeight: '700' },
  reportRow: {
    borderBottomWidth: 1,
    borderBottomColor: colors.border,
    paddingVertical: 12,
    paddingHorizontal: 8,
  },
  reportRowAlt: { backgroundColor: colors.surfaceAlt },
  reportHeader: { flexDirection: 'row', justifyContent: 'space-between', marginBottom: 4 },
  reportId: { fontSize: 12, color: colors.textSecondary },
  reportTotal: { fontSize: 14, fontWeight: '700', color: colors.primary },
  reportCustomer: { fontSize: 14, color: colors.textPrimary, fontWeight: '600' },
  reportCity: { fontSize: 12, color: colors.textSecondary },
  reportDate: { fontSize: 12, color: colors.textSecondary, marginTop: 2 },
  reportPayment: { fontSize: 12, color: colors.textSecondary },
  pdfButton: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
    paddingHorizontal: 10,
    paddingVertical: 5,
    borderWidth: 1,
    borderColor: colors.primary,
    borderRadius: 6,
    alignSelf: 'flex-start',
    marginTop: 6,
  },
  pdfButtonText: { fontSize: 12, color: colors.primary, fontWeight: '600' },
  emptyText: { textAlign: 'center', color: colors.textSecondary, paddingVertical: 24, fontSize: 14 },
});

export function BudgetReportScreen({ onGoBack }: BudgetReportScreenProps) {
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [data, setData] = useState<IBudgetReportListing | null>(null);

  const handleSearch = async () => {
    setIsLoading(true);
    try {
      const filter: IBudgetReportFilter = {
        registersStart: 0,
        registersEnd: 50,
        creationDateTimeStart: startDate || undefined,
        creationDateTimeEnd: endDate || undefined,
      };
      const result = await budgetReportService.getReport(filter);
      setData(result);
    } catch {
      // Error handled by service
    } finally {
      setIsLoading(false);
    }
  };

  const handleGeneratePdf = async (report: IBudgetReport) => {
    if (Platform.OS !== 'web') return;
    try {
      const template = await templateService.getTemplate('document');
      const html = renderTemplate(template, {
        DocumentTitle: 'Orçamento',
        Id: report.id,
        CustomerName: report.customerName,
        CreationDateTime: new Date(report.creationDateTime).toLocaleDateString('pt-BR'),
        Observations: report.observations ?? '',
        ValueSubtotal: report.valueSubtotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }),
        ValueTotal: report.valueTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }),
        PaymentMethod: report.paymentMethodDescription,
      });
      const win = (window as Window).open('', '_blank');
      if (win) {
        win.document.write(html);
        win.document.close();
        win.focus();
        win.print();
      }
    } catch {
      // Handled by service
    }
  };

  const formatCurrency = (v: number) => v.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  const formatDate = (s: string) => new Date(s).toLocaleDateString('pt-BR');

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={[layoutStyles.scrollContainer, { justifyContent: 'flex-start' }]}>
        <Breadcrumb items={[
          { label: 'Início', onPress: onGoBack },
          { label: 'Relatórios', onPress: onGoBack },
          { label: 'Orçamentos' },
        ]} />
        <Text style={typographyStyles.title}>Relatório de Orçamentos</Text>

        <View style={styles.sectionCard}>
          <View style={styles.filterRow}>
            <View style={styles.filterField}>
              <DateField label="Data início" value={startDate} onChangeText={setStartDate} />
            </View>
            <View style={styles.filterField}>
              <DateField label="Data fim" value={endDate} onChangeText={setEndDate} />
            </View>
            <PrimaryButton title="Gerar" isLoading={isLoading} onPress={handleSearch} style={styles.actionBtn} />
          </View>
        </View>

        {data?.summary && (
          <View style={styles.summaryBox}>
            <Text style={styles.summaryLabel}>Total Geral</Text>
            <Text style={styles.summaryValue}>{formatCurrency(data.summary.valueTotal)}</Text>
          </View>
        )}

        {isLoading && (
          <View style={{ paddingVertical: 32, alignItems: 'center' }}>
            <ActivityIndicator color={colors.primary} />
          </View>
        )}

        {!isLoading && data && (
          <View style={styles.sectionCard}>
            {data.registers.length === 0 ? (
              <Text style={styles.emptyText}>Nenhum registro encontrado.</Text>
            ) : (
              data.registers.map((report, idx) => (
                <View key={report.id} style={[styles.reportRow, idx % 2 !== 0 ? styles.reportRowAlt : null]}>
                  <View style={styles.reportHeader}>
                    <Text style={styles.reportId}>#{report.id}</Text>
                    <Text style={styles.reportTotal}>{formatCurrency(report.valueTotal)}</Text>
                  </View>
                  <Text style={styles.reportCustomer}>{report.customerName}</Text>
                  <Text style={styles.reportCity}>{report.customerCityState}</Text>
                  <Text style={styles.reportDate}>{formatDate(report.creationDateTime)}</Text>
                  <Text style={styles.reportPayment}>{report.paymentMethodDescription}</Text>
                  {Platform.OS === 'web' && (
                    <TouchableOpacity style={styles.pdfButton} onPress={() => handleGeneratePdf(report)} activeOpacity={0.7}>
                      <Text style={styles.pdfButtonText}>📄 Gerar PDF</Text>
                    </TouchableOpacity>
                  )}
                </View>
              ))
            )}
          </View>
        )}
      </ScrollView>
    </View>
  );
}
