import { Article, RocketLaunch } from '@mui/icons-material';
import { AppBar, Box, Button, Container, IconButton, Toolbar, Typography } from '@mui/material';
import * as React from 'react';
import { Link, useNavigate } from 'react-router-dom';

const TopNav = () => {

  let navigate = useNavigate();

  return (
    <Container disableGutters={true} className="top-nav" fixed maxWidth="lg">
      <Box sx={{ flexGrow: 1 }}>
        <AppBar position="static" >
          <Toolbar >
            <IconButton onClick={() => { navigate("/") }} size="large" edge="start" color="inherit" aria-label="menu" sx={{ mr: 2 }} >
              <RocketLaunch />
              <Typography variant="h6" flexGrow={0} >&nbsp; Azure Function SPA Demo</Typography>
            </IconButton>
            <Box display='flex' flexGrow={8} >
              <Button onClick={() => { navigate("workspaces") }} color="inherit">User Workspaces</Button>
              <Button onClick={() => { navigate("adminworkspaces") }} color="inherit">Admin Workspaces</Button>
              <Button onClick={() => { navigate("embeddedreport") }} color="inherit">Embedded Report</Button>
            </Box>
          </Toolbar>
        </AppBar>
      </Box>
    </Container>
  )
}

export default TopNav;