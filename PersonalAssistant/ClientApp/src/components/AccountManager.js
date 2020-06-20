import React from 'react';
import { connect } from "react-redux";
import { Route } from 'react-router';
import { Link, withRouter } from 'react-router-dom';
import { Tabs, Tab } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

import RestfulTable from './RestfulTable';
import { TabPaths } from '../helper/RouterConstants'
import Dashboard from './Dashboard';

function a11yProps(index) {
  return {
    id: `vertical-tab-${index}`,
    'aria-controls': `vertical-tabpanel-${index}`,
  };
}

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    display: 'flex',
    height: '100vh',
    backgroundColor: theme.palette.background.paper,
  },
  tabs: {
    borderRight: `1px solid ${theme.palette.divider}`,
  },
  tabPanel: {
    width: '100vw',
  }
}));
const AccountManager = props => {
  const classes = useStyles();
  let defaultValue = 0;
  switch (props.location.pathname) {
    case TabPaths.AccountManager.Index:
    case TabPaths.AccountManager.Dashboard:
      defaultValue = 0;
      break;
    case TabPaths.AccountManager.Expenditures:
      defaultValue = 1;
      break;
    case TabPaths.AccountManager.Incomes:
      defaultValue = 2;
      break;
    case TabPaths.AccountManager.StockTransactions:
      defaultValue = 3;
      break;
    case TabPaths.AccountManager.InternalTransfers:
      defaultValue = 4;
      break;
    case TabPaths.AccountManager.ExpenditureTypes:
      defaultValue = 5;
      break;
    case TabPaths.AccountManager.AccountInitializations:
      defaultValue = 6;
      break;
    case TabPaths.AccountManager.StockInitializations:
      defaultValue = 7;
      break;
    default:
      break;
  }
  const [value, setValue] = React.useState(defaultValue);
  const { translate } = props;
  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  return (
    <div className={classes.root}>
      <Tabs
        orientation="vertical"
        variant="scrollable"
        value={value}
        onChange={handleChange}
        aria-label="Vertical tabs example"
        className={classes.tabs}
      >
        <Tab label={translate.dashboard} component={Link} to={TabPaths.AccountManager.Dashboard} {...a11yProps(0)} />
        <Tab label={translate.expenditure} component={Link} to={TabPaths.AccountManager.Expenditures} {...a11yProps(1)} />
        <Tab label={translate.income} component={Link} to={TabPaths.AccountManager.Incomes} {...a11yProps(2)} />
        <Tab label={translate.internalTransfer} component={Link} to={TabPaths.AccountManager.InternalTransfers} {...a11yProps(3)} />
        <Tab label={translate.stockTransaction} component={Link} to={TabPaths.AccountManager.StockTransactions} {...a11yProps(4)} />
        <Tab label={translate.expenditureType} component={Link} to={TabPaths.AccountManager.ExpenditureTypes} {...a11yProps(5)} />
        <Tab label={translate.accountInitialization} component={Link} to={TabPaths.AccountManager.AccountInitializations} {...a11yProps(6)} />
        <Tab label={translate.stockInitialization} component={Link} to={TabPaths.AccountManager.StockInitializations} {...a11yProps(7)} />
      </Tabs>
      <Route exact path={`${props.match.path}/Dashboard`} render={() => <Dashboard />} />
      <Route path={`${props.match.path}/Expenditures`} render={() => <RestfulTable controller='Expenditures' />} />
      <Route path={`${props.match.path}/Incomes`} render={() => <RestfulTable controller='Incomes' />} />
      <Route path={`${props.match.path}/InternalTransfers`} render={() => <RestfulTable controller='InternalTransfers' />} />
      <Route path={`${props.match.path}/StockTransactions`} render={() => <RestfulTable controller='StockTransactions' />} />
      <Route path={`${props.match.path}/ExpenditureTypes`} render={() => <RestfulTable controller='ExpenditureTypes' />} />
      <Route path={`${props.match.path}/AccountInitializations`} render={() => <RestfulTable controller='AccountInitializations' />} />
      <Route path={`${props.match.path}/StockInitializations`} render={() => <RestfulTable controller='StockInitializations' />} />
    </div>
  );
}

const mapStateToProps = store => ({
  translate: {
    dashboard: store.lang.translation.accountManager.Dashboard,
    expenditureType: store.lang.translation.accountManager.ExpenditureType,
    accountInitialization: store.lang.translation.accountManager.AccountInitialization,
    expenditure: store.lang.translation.accountManager.Expenditure,
    income: store.lang.translation.accountManager.Income,
    internalTransfer: store.lang.translation.accountManager.InternalTransfer,
    stockInitialization: store.lang.translation.accountManager.StockInitialization,
    stockTransaction: store.lang.translation.accountManager.StockTransaction,
  }
});
export default withRouter(connect(mapStateToProps)(AccountManager));