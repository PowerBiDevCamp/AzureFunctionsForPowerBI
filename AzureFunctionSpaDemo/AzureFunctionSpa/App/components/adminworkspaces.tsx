import * as React from 'react';

import { useState } from 'react';

import { AdminWorkspace } from '../models/models';
import AzureFunctionApi from '../services/AzureFunctionApi';

import Button from '@mui/material/Button';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';

const AdminWorkspaces = () => {

  const [workspaces, setWorkspaces] = useState<AdminWorkspace[]>([] as AdminWorkspace[]);

  let getData = async () => {

    console.log("Calling GetData...");
    let workspacesResponse = await AzureFunctionApi.GetWorkspacesAsAdmin();
    console.log("Calling SetWorkspace...", workspacesResponse);

    setWorkspaces(workspacesResponse);

  };

  return (
    <div>
      <h2>Admin Workspaces</h2>

      <div>
        <Button
          variant="contained"
          color="primary"
          onClick={getData}>Get Workspaces using Power BI Admin API</Button>
      </div>

      {(workspaces.length > 0) ? (
        <TableContainer component={Paper}>
          <Table aria-label="simple table" sx={{ marginTop: "12px" }}>
            <TableHead sx={{ "& th": { color: "white", backgroundColor: "black" } }} >
                <TableRow>
                <TableCell>Name</TableCell>
                <TableCell>ID</TableCell>
                <TableCell>Capacity ID</TableCell>
                <TableCell>Datasets</TableCell>
                <TableCell>Reports</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {workspaces.map((workspace) => (
                <TableRow key={workspace.id}>
                  <TableCell component="th" scope="row">{workspace.name}</TableCell>
                  <TableCell>{workspace.id}</TableCell>
                  <TableCell>{workspace.capacityId}</TableCell>
                  <TableCell>{workspace.reports.length}</TableCell>
                  <TableCell>{workspace.datasets.length}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>) : 
        <div className="information-message">No workspaces have been loaded yet</div>
      }
    </div>
  )
}

export default AdminWorkspaces;