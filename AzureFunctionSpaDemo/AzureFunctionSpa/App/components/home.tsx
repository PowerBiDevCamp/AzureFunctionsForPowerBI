import * as React from 'react';

const Home = () => {
  return (
    <div>
      <h2>Welcome to the Azure Function App SPA Demo</h2>
      <h4>Here are the technqiues that this sample application demonstrates</h4>
      <ul>
        <li>Calling an Azure function to return workspaces using the User API</li>
        <li>Calling an Azure function to return workspaces using the Admin API</li>
        <li>Calling an Azure function to return embedding data to embed a Power BI report</li>
      </ul>
    </div>
  )
}

export default Home;