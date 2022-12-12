export enum FormControlErrorType {
  Required = 0,
  Pattern
}

export enum PageStatus {
  Create = 1,
  Edit = 2,
  Review = 3,
}

export enum StatusCode {
  Success = 0,
  Fail = 1,
}

export enum Group {
  系統管理員 = 1,
  一般用戶 = 2,
}

export enum UnitID {
  帳戶管理 = 1,
  單元管理 = 4,
  審核列表 = 6,
  首頁設定 = 7,
  Footer設定 = 8,
  作品集 = 9,
  客戶服務 = 10,
  常見問題 = 11,
  聯絡我們 = 12,
  關於我們 = 13,
}

export enum TemplateType {
  固定單元 = -1,
  無 = 0,
  模板一 = 1,
  模板二 = 2,
}

export enum StageType {
  前後台 = 0,
  前台 = 1,
  後台 = 2,
}

export enum Right {
  C_R_U_D = 1,
  C = 2,
  R = 3,
  U = 4,
  D = 5
}

export enum EditStatus {
  Review = 1,
  Reject = 2,
  Approve = 3
}

export enum EnabledOptions {
  全部 = -1,
  啟用 = 1,
  停用 = 0
}

export enum EditAndEnabledOptions {
  全部 = -1,
  停用 = 0,
  啟用 = 1,
  審核中 = 2,
  駁回 = 3,
}

export enum UploadPhotoType{
  單元背景圖 = 1,
  常見問題 = 2,
  Type1 = 3,
  Type2 = 4,
}
