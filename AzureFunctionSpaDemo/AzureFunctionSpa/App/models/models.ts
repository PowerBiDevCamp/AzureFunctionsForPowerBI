
export interface Workspace {
  id: string;
  name: string;
  isOnDedicatedCapacity: boolean;
  capacityId: string;
}

export interface AdminWorkspace {
  id: string;
  name: string;
  isReadOnly: boolean;
  isOnDedicatedCapacity: boolean;
  capacityId: string;
  type: string;
  state: string;
  users: User[];
  reports: Report[];
  datasets: Dataset[];
  pipelineId: string;
}

export interface EmbeddedViewModel {
  reportId: string;
  embedUrl: string;
  embedToken: string;
}

export interface User {
  groupUserAccessRight: string;
  emailAddress: string;
  displayName: string;
  identifier: string;
  principalType: string;
}

export interface Report {
  id: string;
  name: string;
  datasetId: string;
  reportType: string;
  webUrl?: string;
  embedUrl?: string;
  createdBy?: string;
  modifiedBy?: string;
  createdDateTime: Date;
  modifiedDateTime: Date;
}

export interface Dataset {
  id: string;
  name: string;
  configuredBy: string;
  CreatedDate: Date;
  ContentProviderType: string;
  IsEffectiveIdentityRequired: boolean;
  IsEffectiveIdentityRolesRequired: boolean;
  IsOnPremGatewayRequired?: boolean;
  IsRefreshable: boolean;
  targetStorageMode: string;
}