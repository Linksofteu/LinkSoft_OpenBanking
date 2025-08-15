/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export enum BankTransactionCodeIssuer {
  CBA = "CBA",
  OTHER = "OTHER",
}

export enum TransactionType {
  INTEREST = "INTEREST",
  FEE = "FEE",
  DOMESTIC = "DOMESTIC",
  FOREIGN = "FOREIGN",
  SEPA = "SEPA",
  CASH = "CASH",
  CARD = "CARD",
  OTHER = "OTHER",
}

export enum CreditDebitIndicator {
  CREDIT = "CREDIT",
  DEBIT = "DEBIT",
}

export enum AccountType {
  KB = "KB",
  AG = "AG",
}

export interface AccountDirectAccessApplicationManifest {
  /** @format guid */
  id: string;
  targetEnvironment: string;
  softwareStatementRegistrationDocument: SoftwareStatementRequest;
  softwareStatement?: AccountDirectAccessApplicationManifestSoftwareStatementRegistrationResult | null;
  applicationRegistration?: AccountDirectAccessApplicationManifestApplicationRegistrationResult | null;
  applicationAuthorization?: AccountDirectAccessApplicationManifestApplicationAuthorizationResult | null;
}

export interface SoftwareStatementRequest {
  /**
   * @minLength 5
   * @maxLength 50
   */
  softwareName: string;
  /**
   * @minLength 5
   * @maxLength 50
   */
  softwareNameEn: string;
  /**
   * @minLength 0
   * @maxLength 64
   */
  softwareId: string;
  /**
   * @minLength 1
   * @maxLength 30
   */
  softwareVersion: string;
  /** @format uri */
  softwareUri: string;
  redirectUris: string[];
  tokenEndpointAuthMethod: string;
  grantTypes: string[];
  responseTypes: string[];
  /** @format uri */
  registrationBackUri: string;
  /**
   * @maxItems 2
   * @minItems 1
   */
  contacts: string[];
  /** @format uri */
  logoUri: string;
  /** @format uri */
  tosUri: string;
  /** @format uri */
  policyUri: string;
  [key: string]: any;
}

export interface AccountDirectAccessApplicationManifestSoftwareStatementRegistrationResult {
  jwt: string;
  /** @format date-time */
  validToUtc: string;
}

export interface AccountDirectAccessApplicationManifestApplicationRegistrationResult {
  clientId: string;
  clientSecret: string;
  registrationClientUri?: string | null;
}

export interface AccountDirectAccessApplicationManifestApplicationAuthorizationResult {
  refreshToken: string;
  /** @format date-time */
  refreshTokenExpirationUtc: string;
}

export interface SoftwareStatementRegistrationDocumentModel {
  generateSoftwareId: boolean;
  /**
   * @minLength 0
   * @maxLength 50
   */
  softwareName: string;
  /**
   * @minLength 0
   * @maxLength 50
   */
  softwareNameEn: string;
  /**
   * @minLength 0
   * @maxLength 64
   */
  softwareId: string;
  /**
   * @minLength 0
   * @maxLength 30
   */
  softwareVersion: string;
  /** @minLength 1 */
  contacts: string[];
  /** @minLength 1 */
  softwareUri: string;
  /** @minLength 1 */
  policyUri: string;
  /** @minLength 1 */
  tosUri: string;
  /** @minLength 1 */
  logoUri: string;
  /** @minLength 1 */
  registrationBackUri: string;
  /** @minLength 1 */
  redirectUris: string[];
}

/** the dto used to send an error response to the client */
export interface ErrorResponse {
  /**
   * the http status code sent to the client. default is 400.
   * @format int32
   * @default 400
   */
  statusCode: number;
  /**
   * the message for the error response
   * @default "One or more errors occurred!"
   */
  message: string;
  /** the collection of errors for the current context */
  errors: Record<string, string[]>;
}

export interface Account {
  /**
   * @minLength 0
   * @maxLength 400
   */
  accountId: string;
  /**
   * @minLength 0
   * @maxLength 34
   */
  iban: string;
  /**
   * @minLength 0
   * @maxLength 3
   */
  currency: string;
  nameI18N?: string | null;
  productI18N?: string | null;
  [key: string]: any;
}

export type ApplicationRequestBase = object;

export interface PageSlice {
  content: AccountTransaction[];
  /** @format int32 */
  totalPages: number;
  /** @format int32 */
  pageNumber: number;
  /** @format int32 */
  pageSize: number;
  /** @format int32 */
  numberOfElements: number;
  first: boolean;
  last: boolean;
  empty: boolean;
  [key: string]: any;
}

export interface AccountTransaction {
  /** @format date-time */
  lastUpdated: string;
  accountType: AccountType;
  entryReference?: string | null;
  /**
   * @minLength 0
   * @maxLength 34
   */
  iban: string;
  creditDebitIndicator: CreditDebitIndicator;
  transactionType: TransactionType;
  bankTransactionCode?: BankTransactionCode | null;
  amount: CurrencyAmount;
  /** @format date-time */
  bookingDate?: string | null;
  /** @format date-time */
  valueDate?: string | null;
  instructed?: CurrencyAmount | null;
  reversalIndicator?: boolean | null;
  status?: string | null;
  counterParty?: TransactionCounterparty | null;
  references?: TransactionReferences | null;
  /**
   * @minLength 0
   * @maxLength 500
   */
  additionalTransactionInformation?: string | null;
  cardTransactionDetails?: CardTransactionDetails | null;
  [key: string]: any;
}

export interface BankTransactionCode {
  /**
   * @minLength 0
   * @maxLength 35
   */
  code?: string | null;
  issuer?: BankTransactionCodeIssuer | null;
  [key: string]: any;
}

export interface CurrencyAmount {
  /** @format double */
  value?: number | null;
  /**
   * @minLength 0
   * @maxLength 3
   */
  currency?: string | null;
  [key: string]: any;
}

export interface TransactionCounterparty {
  /**
   * @minLength 0
   * @maxLength 34
   */
  iban?: string | null;
  name?: string | null;
  /**
   * @minLength 0
   * @maxLength 34
   */
  accountNo?: string | null;
  /**
   * @minLength 0
   * @maxLength 11
   */
  bankBic?: string | null;
  /**
   * @minLength 0
   * @maxLength 4
   */
  bankCode?: string | null;
  bankName?: string | null;
  [key: string]: any;
}

export interface TransactionReferences {
  accountServicer?: string | null;
  endToEndIdentification?: string | null;
  variable?: string | null;
  constant?: string | null;
  specific?: string | null;
  receiver?: string | null;
  myDescription?: string | null;
  [key: string]: any;
}

export interface CardTransactionDetails {
  /** @format date-time */
  holdExpirationDate?: string | null;
  [key: string]: any;
}

export type GetAccountTransactionsRequest = ApplicationRequestBase & object;

export interface HandleCallbackDataResponse {
  redirectUri?: string | null;
  updatedApplication: AccountDirectAccessApplicationManifest;
}

export interface HandleCallbackDataRequest {
  /** @minLength 1 */
  encryptedData: string;
  /** @minLength 1 */
  salt: string;
  /** @minLength 1 */
  code: string;
  /** @minLength 1 */
  state: string;
  [key: string]: any;
}

export interface RegisterSoftwareStatementRequest {
  /**
   * @format guid
   * @minLength 1
   */
  applicationId: string;
}

export type QueryParamsType = Record<string | number, any>;
export type ResponseFormat = keyof Omit<Body, "body" | "bodyUsed">;

export interface FullRequestParams extends Omit<RequestInit, "body"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseFormat;
  /** request body */
  body?: unknown;
  /** base url */
  baseUrl?: string;
  /** request cancellation token */
  cancelToken?: CancelToken;
}

export type RequestParams = Omit<
  FullRequestParams,
  "body" | "method" | "query" | "path"
>;

export interface ApiConfig<SecurityDataType = unknown> {
  baseUrl?: string;
  baseApiParams?: Omit<RequestParams, "baseUrl" | "cancelToken" | "signal">;
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<RequestParams | void> | RequestParams | void;
  customFetch?: typeof fetch;
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown>
  extends Response {
  data: D;
  error: E;
}

type CancelToken = Symbol | string | number;

export enum ContentType {
  Json = "application/json",
  JsonApi = "application/vnd.api+json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public baseUrl: string = "";
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private abortControllers = new Map<CancelToken, AbortController>();
  private customFetch = (...fetchParams: Parameters<typeof fetch>) =>
    fetch(...fetchParams);

  private baseApiParams: RequestParams = {
    credentials: "same-origin",
    headers: {},
    redirect: "follow",
    referrerPolicy: "no-referrer",
  };

  constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
    Object.assign(this, apiConfig);
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected encodeQueryParam(key: string, value: any) {
    const encodedKey = encodeURIComponent(key);
    return `${encodedKey}=${encodeURIComponent(typeof value === "number" ? value : `${value}`)}`;
  }

  protected addQueryParam(query: QueryParamsType, key: string) {
    return this.encodeQueryParam(key, query[key]);
  }

  protected addArrayQueryParam(query: QueryParamsType, key: string) {
    const value = query[key];
    return value.map((v: any) => this.encodeQueryParam(key, v)).join("&");
  }

  protected toQueryString(rawQuery?: QueryParamsType): string {
    const query = rawQuery || {};
    const keys = Object.keys(query).filter(
      (key) => "undefined" !== typeof query[key],
    );
    return keys
      .map((key) =>
        Array.isArray(query[key])
          ? this.addArrayQueryParam(query, key)
          : this.addQueryParam(query, key),
      )
      .join("&");
  }

  protected addQueryParams(rawQuery?: QueryParamsType): string {
    const queryString = this.toQueryString(rawQuery);
    return queryString ? `?${queryString}` : "";
  }

  private contentFormatters: Record<ContentType, (input: any) => any> = {
    [ContentType.Json]: (input: any) =>
      input !== null && (typeof input === "object" || typeof input === "string")
        ? JSON.stringify(input)
        : input,
    [ContentType.JsonApi]: (input: any) =>
      input !== null && (typeof input === "object" || typeof input === "string")
        ? JSON.stringify(input)
        : input,
    [ContentType.Text]: (input: any) =>
      input !== null && typeof input !== "string"
        ? JSON.stringify(input)
        : input,
    [ContentType.FormData]: (input: any) =>
      Object.keys(input || {}).reduce((formData, key) => {
        const property = input[key];
        formData.append(
          key,
          property instanceof Blob
            ? property
            : typeof property === "object" && property !== null
              ? JSON.stringify(property)
              : `${property}`,
        );
        return formData;
      }, new FormData()),
    [ContentType.UrlEncoded]: (input: any) => this.toQueryString(input),
  };

  protected mergeRequestParams(
    params1: RequestParams,
    params2?: RequestParams,
  ): RequestParams {
    return {
      ...this.baseApiParams,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...(this.baseApiParams.headers || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected createAbortSignal = (
    cancelToken: CancelToken,
  ): AbortSignal | undefined => {
    if (this.abortControllers.has(cancelToken)) {
      const abortController = this.abortControllers.get(cancelToken);
      if (abortController) {
        return abortController.signal;
      }
      return void 0;
    }

    const abortController = new AbortController();
    this.abortControllers.set(cancelToken, abortController);
    return abortController.signal;
  };

  public abortRequest = (cancelToken: CancelToken) => {
    const abortController = this.abortControllers.get(cancelToken);

    if (abortController) {
      abortController.abort();
      this.abortControllers.delete(cancelToken);
    }
  };

  public request = async <T = any, E = any>({
    body,
    secure,
    path,
    type,
    query,
    format,
    baseUrl,
    cancelToken,
    ...params
  }: FullRequestParams): Promise<HttpResponse<T, E>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.baseApiParams.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const queryString = query && this.toQueryString(query);
    const payloadFormatter = this.contentFormatters[type || ContentType.Json];
    const responseFormat = format || requestParams.format;

    return this.customFetch(
      `${baseUrl || this.baseUrl || ""}${path}${queryString ? `?${queryString}` : ""}`,
      {
        ...requestParams,
        headers: {
          ...(requestParams.headers || {}),
          ...(type && type !== ContentType.FormData
            ? { "Content-Type": type }
            : {}),
        },
        signal:
          (cancelToken
            ? this.createAbortSignal(cancelToken)
            : requestParams.signal) || null,
        body:
          typeof body === "undefined" || body === null
            ? null
            : payloadFormatter(body),
      },
    ).then(async (response) => {
      const r = response.clone() as HttpResponse<T, E>;
      r.data = null as unknown as T;
      r.error = null as unknown as E;

      const data = !responseFormat
        ? r
        : await response[responseFormat]()
            .then((data) => {
              if (r.ok) {
                r.data = data;
              } else {
                r.error = data;
              }
              return r;
            })
            .catch((e) => {
              r.error = e;
              return r;
            });

      if (cancelToken) {
        this.abortControllers.delete(cancelToken);
      }

      if (!response.ok) throw data;
      return data;
    });
  };
}

/**
 * @title OpenBanking Workbench API
 * @version v1
 */
export class Api<
  SecurityDataType extends unknown,
> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags Applications
     * @name CreateApplicationEndpoint
     * @request POST:/api/applications
     * @response `200` `AccountDirectAccessApplicationManifest` Success
     * @response `400` `ErrorResponse` Bad Request
     */
    createApplicationEndpoint: (
      data: SoftwareStatementRegistrationDocumentModel,
      params: RequestParams = {},
    ) =>
      this.request<AccountDirectAccessApplicationManifest, ErrorResponse>({
        path: `/api/applications`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Applications
     * @name GetAllApplicationsEndpoint
     * @request GET:/api/applications
     * @response `200` `(AccountDirectAccessApplicationManifest)[]` Success
     */
    getAllApplicationsEndpoint: (params: RequestParams = {}) =>
      this.request<AccountDirectAccessApplicationManifest[], any>({
        path: `/api/applications`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Applications
     * @name GetAccountsEndpoint
     * @request GET:/api/applications/{applicationId}/accounts
     * @response `200` `(Account)[]` Success
     * @response `400` `ErrorResponse` Bad Request
     */
    getAccountsEndpoint: (applicationId: string, params: RequestParams = {}) =>
      this.request<Account[], ErrorResponse>({
        path: `/api/applications/${applicationId}/accounts`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Applications
     * @name GetAccountTransactionsEndpoint
     * @request GET:/api/applications/{applicationId}/accounts/{accountId}/transactions
     * @response `200` `PageSlice` Success
     * @response `400` `ErrorResponse` Bad Request
     */
    getAccountTransactionsEndpoint: (
      applicationId: string,
      accountId: string,
      params: RequestParams = {},
    ) =>
      this.request<PageSlice, ErrorResponse>({
        path: `/api/applications/${applicationId}/accounts/${accountId}/transactions`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Applications
     * @name GetApplicationAuthorizationUrlEndpoint
     * @request GET:/api/applications/{applicationId}/authorizationUrl
     * @response `200` `string` Success
     * @response `400` `void` Bad Request
     * @response `404` `void` Not Found
     */
    getApplicationAuthorizationUrlEndpoint: (
      applicationId: string,
      params: RequestParams = {},
    ) =>
      this.request<string, void>({
        path: `/api/applications/${applicationId}/authorizationUrl`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Applications
     * @name GetApplicationRegistrationUrlEndpoint
     * @request GET:/api/applications/{applicationId}/registrationUrl
     * @response `200` `string` Success
     * @response `400` `void` Bad Request
     * @response `404` `void` Not Found
     */
    getApplicationRegistrationUrlEndpoint: (
      applicationId: string,
      params: RequestParams = {},
    ) =>
      this.request<string, void>({
        path: `/api/applications/${applicationId}/registrationUrl`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Applications
     * @name HandleCallbackFlowDataEndpoint
     * @request POST:/api/applications/handleCallbackFlow
     * @response `200` `HandleCallbackDataResponse` Success
     * @response `400` `ErrorResponse` Bad Request
     */
    handleCallbackFlowDataEndpoint: (
      data: HandleCallbackDataRequest,
      params: RequestParams = {},
    ) =>
      this.request<HandleCallbackDataResponse, ErrorResponse>({
        path: `/api/applications/handleCallbackFlow`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Applications
     * @name RegisterSoftwareStatementEndpoint
     * @request PATCH:/api/applications/registerSoftwareStatement
     * @response `200` `AccountDirectAccessApplicationManifest` Success
     * @response `400` `ErrorResponse` Bad Request
     * @response `404` `ErrorResponse` Not Found
     */
    registerSoftwareStatementEndpoint: (
      data: RegisterSoftwareStatementRequest,
      params: RequestParams = {},
    ) =>
      this.request<AccountDirectAccessApplicationManifest, ErrorResponse>({
        path: `/api/applications/registerSoftwareStatement`,
        method: "PATCH",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
}
