import React, { Component } from 'react';
import { Route } from 'react-router';

import Layout from './components/Layout';
import PasswordManager from './components/PasswordManager';
import AccountManager from './components/AccountManager';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import { TabPaths } from './helper/RouterConstants';
import Home from './components/Home';
import TempForTestApi from './components/TempForTestApi';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path={TabPaths.Default} component={Home} />
        <Route path={TabPaths.PasswordManager} component={PasswordManager} />
        <AuthorizeRoute path={TabPaths.AccountManager.Index} component={AccountManager} />
        <AuthorizeRoute path={'/testapi'} component={TempForTestApi} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}