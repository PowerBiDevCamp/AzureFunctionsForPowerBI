import * as React from 'react';

import TopNav from './components/topnav'
import Home from './components/home';
import Workspaces from './components/workspaces';
import AdminWorkspaces from './components/adminworkspaces';
import EmbeddedReport from './components/embeddedreport';
import { Route, Routes } from "react-router-dom";
import { CssBaseline } from '@mui/material';
import Container from '@mui/material/Container';

import './App.css';

const App = () => {

  return (
    <React.Fragment>
      <CssBaseline />
      <div className="top-div" >
        <TopNav />
        <Container className="content-box" fixed maxWidth="lg">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="workspaces" element={<Workspaces />} />
            <Route path="adminworkspaces" element={<AdminWorkspaces />} />
            <Route path="embeddedreport" element={<EmbeddedReport />} />
          </Routes>
        </Container>
      </div>
    </React.Fragment> )
}

export default App;