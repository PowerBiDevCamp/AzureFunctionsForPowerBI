import * as React from 'react';

import { EmbeddedViewModel } from '../models/models';
import AzureFunctionApi from '../services/AzureFunctionApi';

import * as powerbi from "powerbi-client";
import * as models from "powerbi-models";

// ensure Power BI JavaScript API has loaded
require('powerbi-models');
require('powerbi-client');


import Button from '@mui/material/Button';

const EmbeddedReport = () => {

  let GetEmbeddingData = async () => {

    console.log("Calling GetData...");
    let viewModel : EmbeddedViewModel = await AzureFunctionApi.GetEmbeddingData();
    console.log("Calling GetEmbeddingData", viewModel);

		var reportContainer: HTMLElement = document.getElementById("embed-container");

		var config: powerbi.IEmbedConfiguration = {
			type: 'report',
			id: viewModel.reportId,
			embedUrl: viewModel.embedUrl,
			accessToken: viewModel.embedToken,
			permissions: models.Permissions.All,
			tokenType: models.TokenType.Embed,
			viewMode: models.ViewMode.View,
			settings: {
				panes: {
					filters: { expanded: false, visible: true },
					pageNavigation: { visible: true, position: models.PageNavigationPosition.Left }
				}
			}
		};

		// Embed the report and display it within the div container.
		var report = window.powerbi.embed(reportContainer, config);

		// display report properties in browser console
		console.log(report);

  };

  return (
    <div>
   
			<div style={{ "marginTop" : "24px" }}>
        <Button
          variant="contained"
          color="primary"
          onClick={GetEmbeddingData}>Get Embedding Data for Power BI Report</Button>
      </div>

      <div id="embed-container" />


    </div>)
};

export default EmbeddedReport;