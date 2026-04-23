import { ListingFilter } from './ListingFilter';

export interface DocumentReportFilter extends ListingFilter {
  creationDateTimeStart?: string;
  creationDateTimeEnd?: string;
  lastUpdateDateTimeStart?: string;
  lastUpdateDateTimeEnd?: string;
}
