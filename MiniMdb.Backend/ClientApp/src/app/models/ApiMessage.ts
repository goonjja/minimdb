/**
 * Base API message envelope
 */
export class ApiMessageBase {
  error: ApiError;
  pagination: ApiPagination;
}

/**
 * API message envelope with data
 */
export class ApiMessage<T> extends ApiMessageBase {
  data: T[];
}

/**
 * API error description
 */
export class ApiError {
  code: number;
  message: string;
}

/**
 * Pagination details
 */
export class ApiPagination {
  count: number;
  page: number;
  pageSize: number;
  totalPages: number;
}
