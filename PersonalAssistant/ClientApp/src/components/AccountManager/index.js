import React from 'react';
import { connect } from "react-redux";
import { Route } from 'react-router';
import { Link, withRouter } from 'react-router-dom';
import { makeStyles } from '@material-ui/core/styles';
import Tab from '@material-ui/core/Tab';
import Tabs from '@material-ui/core/Tabs';
import Container from '@material-ui/core/Container';

import RestfulTable from '../RestfulTable';
import { TabPaths } from '../../helper/RouterConstants'
import Dashboard from '../Dashboard';
import Expenditure from './Expenditure';
import EditDialog from './EditDialog';

const useStyles = makeStyles((theme) => ({
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
    <Container>
      <Tabs
        variant='scrollable'
        value={value}
        onChange={handleChange}
        centered
      >
        <Tab label={translate.dashboard} component={Link} to={TabPaths.AccountManager.Dashboard} />
        <Tab label={translate.expenditure} component={Link} to={TabPaths.AccountManager.Expenditures} />
        <Tab label={translate.income} component={Link} to={TabPaths.AccountManager.Incomes} />
        <Tab label={translate.internalTransfer} component={Link} to={TabPaths.AccountManager.InternalTransfers} />
        <Tab label={translate.stockTransaction} component={Link} to={TabPaths.AccountManager.StockTransactions} />
        <Tab label={translate.expenditureType} component={Link} to={TabPaths.AccountManager.ExpenditureTypes} />
        <Tab label={translate.accountInitialization} component={Link} to={TabPaths.AccountManager.AccountInitializations} />
        <Tab label={translate.stockInitialization} component={Link} to={TabPaths.AccountManager.StockInitializations} />
      </Tabs>
      <Route exact path={`${props.match.path}`} render={() => <Dashboard />} />
      <Route path={TabPaths.AccountManager.Dashboard} render={() => <Dashboard />} />
      <Route path={TabPaths.AccountManager.Expenditures} render={() => <Expenditure />} />
      <Route path={TabPaths.AccountManager.Incomes} render={() => <RestfulTable controller='Incomes' />} />
      <Route path={TabPaths.AccountManager.InternalTransfers} render={() => <RestfulTable controller='InternalTransfers' />} />
      <Route path={TabPaths.AccountManager.StockTransactions} render={() => <RestfulTable controller='StockTransactions' />} />
      <Route path={TabPaths.AccountManager.ExpenditureTypes} render={() => <RestfulTable controller='ExpenditureTypes' />} />
      <Route path={TabPaths.AccountManager.AccountInitializations} render={() => <RestfulTable controller='AccountInitializations' />} />
      <Route path={TabPaths.AccountManager.StockInitializations} render={() => <RestfulTable controller='StockInitializations' />} />
    </Container>
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