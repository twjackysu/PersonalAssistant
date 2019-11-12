import React, { Component } from 'react';
import { Route } from 'react-router';

import Layout from './components/Layout';
import PasswordManager from './components/PasswordManager';
import AccountManager from './components/AccountManager';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import { TabPaths } from './helper/RouterConstants';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <AuthorizeRoute exact path={TabPaths.Default} component={AccountManager} />
        <Route path={TabPaths.PasswordManager} component={PasswordManager} />
        <AuthorizeRoute path={TabPaths.AccountManager} component={AccountManager} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}