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

/**
 * The identification of the issuer of the transaction code
 * - CBA - Code of the transaction defined by CBA (Czech Bank Association) associated with specific payment. Every bank uses its own LOV for transaction codes which is derived from 1st to 3rd level of the transaction codes LOV defined by CBA standar for CAMT.053. For more information see https://mojebanka.kb.cz/file/cs/format_xml_vypis_ciselnik_trn.pdf
 * - OTHER
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

/**
 * Indicates whether the account balance is positive or negative
 *
 * CODE:
 * * CREDIT - Accontunt balance is &gt; 0 or Account ballance = 0
 * * DEBIT - Account balance is &lt; 0
 */
export enum CreditDebitIndicator {
  CREDIT = "CREDIT",
  DEBIT = "DEBIT",
}

/**
 * Type of bank account.
 * CODE:
 * * KB - KB accounts
 * * AG - aggregate accounts
 */
export enum AccountType {
  KB = "KB",
  AG = "AG",
}

export interface AccountDirectAccessApplicationManifest {
  /** @format guid */
  id: string;
  targetEnvironment: string;
  /**
   * Request for SoftwareStatement registration.
   *
   */
  softwareStatementRegistrationDocument: SoftwareStatementRequest;
  softwareStatement?: AccountDirectAccessApplicationManifestSoftwareStatementRegistrationResult | null;
  applicationRegistration?: AccountDirectAccessApplicationManifestApplicationRegistrationResult | null;
  applicationAuthorization?: AccountDirectAccessApplicationManifestApplicationAuthorizationResult | null;
}

/**
 * Request for SoftwareStatement registration.
 *
 */
export interface SoftwareStatementRequest {
  /**
   * Software Name in CZ.
   *
   * @minLength 5
   * @maxLength 50
   */
  softwareName: string;
  /**
   * Software Name in EN.
   *
   * @minLength 5
   * @maxLength 50
   */
  softwareNameEn: string;
  /**
   * A unique identifier string (e.g., a Universally Unique Identifier (UUID)) assigned by the client developer or software publisher used by registration endpoints to identify the
   * client software to be dynamically registered.
   *
   * @minLength 0
   * @maxLength 64
   */
  softwareId: string;
  /**
   * A version identifier string for the client software identified by softwareId.  The value of the softwareVersion SHOULD change on any update to the client software identified
   * by the same softwareId.
   *
   * @minLength 1
   * @maxLength 30
   */
  softwareVersion: string;
  /**
   * Software URL.
   *
   * @format uri
   */
  softwareUri: string;
  /**
   * Array of redirection URI strings for use in redirect-based flows such as the authorization code.
   *
   */
  redirectUris: string[];
  /**
   * String indicator of the requested authentication method for the token endpoint.
   *
   */
  tokenEndpointAuthMethod: string;
  /**
   * Array of OAuth 2.0 grant type strings that the client can use.
   *
   */
  grantTypes: string[];
  /**
   * Array of the OAuth 2.0 response type strings that the client can use at the authorization endpoint.
   *
   */
  responseTypes: string[];
  /**
   * URI string representing the endpoint where registration data is sent.
   *
   * @format uri
   */
  registrationBackUri: string;
  /**
   * Array of strings representing ways to contact people responsible for this client, typically email addresses.
   *
   * @maxItems 2
   * @minItems 1
   */
  contacts: string[];
  /**
   * URL string that references a logo for the client.
   *
   * @format uri
   */
  logoUri: string;
  /**
   * URL string that points to a human-readable terms of service document for the client that describes a contractual relationship between the end-user and the client that the
   * end-user accepts when authorizing the client.
   *
   * @format uri
   */
  tosUri: string;
  /**
   * URL string that points to a human-readable privacy policy document that describes how the deployment organization collects, uses, retains, and discloses personal data.
   *
   * @format uri
   */
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

/** The list of transactions on the client's account according to the filter set in the request. */
export interface PageSlice {
  content: AccountTransaction[];
  /**
   * Total number of pages.
   * @format int32
   */
  totalPages: number;
  /**
   * Actual page number. Number of the first page is 0.
   * @format int32
   */
  pageNumber: number;
  /**
   * Size of the page (how many elements are shown per page).
   * @format int32
   */
  pageSize: number;
  /**
   * Number of elements on the current page.
   * @format int32
   */
  numberOfElements: number;
  /** Is the first page. */
  first: boolean;
  /** Is the last page. */
  last: boolean;
  /** Is actual page empty. */
  empty: boolean;
  [key: string]: any;
}

/** The single account transaction details. */
export interface AccountTransaction {
  /**
   * The last transaction history update. Datetime is in ISO 8601 format.
   * @format date-time
   */
  lastUpdated: string;
  /**
   * Type of bank account.
   * CODE:
   * * KB - KB accounts
   * * AG - aggregate accounts
   */
  accountType: AccountType;
  entryReference?: string | null;
  /**
   * @minLength 0
   * @maxLength 34
   */
  iban: string;
  /**
   * Indicates whether the account balance is positive or negative
   *
   * CODE:
   * * CREDIT - Accontunt balance is &gt; 0 or Account ballance = 0
   * * DEBIT - Account balance is &lt; 0
   */
  creditDebitIndicator: CreditDebitIndicator;
  transactionType: TransactionType;
  bankTransactionCode?: BankTransactionCode | null;
  /** Amount with ISO currency. */
  amount: CurrencyAmount;
  /**
   * The date the payment was processed/accounted by bank in ISODate format (''YYYY-MM-DD'').
   * @format date-time
   */
  bookingDate?: string | null;
  /**
   * The payment due date in ISODate format (''YYYY-MM-DD'').
   * @format date-time
   */
  valueDate?: string | null;
  instructed?: CurrencyAmount | null;
  reversalIndicator?: boolean | null;
  /** Status of the payment on account from bank point of view. */
  status?: string | null;
  counterParty?: TransactionCounterparty | null;
  references?: TransactionReferences | null;
  /**
   * Additional information about transaction provided by bank
   * @minLength 0
   * @maxLength 500
   */
  additionalTransactionInformation?: string | null;
  cardTransactionDetails?: CardTransactionDetails | null;
  [key: string]: any;
}

/** Transaction code */
export interface BankTransactionCode {
  /**
   * Transaction code
   * @minLength 0
   * @maxLength 35
   */
  code?: string | null;
  issuer?: BankTransactionCodeIssuer | null;
  [key: string]: any;
}

/** Amount with ISO currency. */
export interface CurrencyAmount {
  /**
   * The amount.
   * @format double
   */
  value?: number | null;
  /**
   * @minLength 0
   * @maxLength 3
   */
  currency?: string | null;
  [key: string]: any;
}

/** Transaction counterparty details. Not all fields may be available for all transactions (e.g. for card transaction). */
export interface TransactionCounterparty {
  /**
   * @minLength 0
   * @maxLength 34
   */
  iban?: string | null;
  /** Name of the counterparty */
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
  /** Counterparty account servicing institution name. */
  bankName?: string | null;
  [key: string]: any;
}

/** Transaction references */
export interface TransactionReferences {
  /** Identification of the payment assigned by bank */
  accountServicer?: string | null;
  /** Unique identification of the payment/transaction provided by the client who initiated the payment */
  endToEndIdentification?: string | null;
  /** Variable symbol */
  variable?: string | null;
  /** Constant symbol */
  constant?: string | null;
  /** Specific symbol */
  specific?: string | null;
  /** Message to a payee / reference to receiver. */
  receiver?: string | null;
  /** Description for me */
  myDescription?: string | null;
  [key: string]: any;
}

/** Details related to a CARD transaction */
export interface CardTransactionDetails {
  /**
   * Expiration date of a pending state transaction. ISO 8601 format.
   * @format date-time
   */
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
