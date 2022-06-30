import { Workspace, AdminWorkspace, EmbeddedViewModel } from "./../models/models"

import AppSetting from "./../appSettings"

class AzureFunctionApi {

  static GetWorkspaces = async (): Promise<Workspace[]> => {
    let restUrl = AppSetting.apiRoot + "GetWorkspaces";
    return fetch(restUrl, { mode: "cors" })
      .then(response => response.json())
      .then(response => { return response });
  }

  static GetWorkspacesAsAdmin = async (): Promise<AdminWorkspace[]> => {
    let restUrl = AppSetting.apiRoot + "GetWorkspacesAsAdminWithExpand";
    return fetch(restUrl, { mode: "cors" })
      .then(response => response.json())
      .then(response => { return response });
  }

  static GetEmbeddingData = async (): Promise<EmbeddedViewModel> => {
    let restUrl = AppSetting.apiRoot + "GetEmbeddingViewModel";
    return fetch(restUrl, { mode: "cors" })
      .then(response => response.json())
      .then(response => { return response });
  }

}

export default AzureFunctionApi;