import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import { AppBar, Hidden, Tabs, Tab } from '@material-ui/core';
import VpnKeyIcon from '@material-ui/icons/VpnKey';
import AccountBalanceIcon from '@material-ui/icons/AccountBalance';
import { TabPaths } from '../helper/RouterConstants'

const NavTabs = props => {
  let defaultValue = 0;
  switch (props.location.pathname) {
    case TabPaths.Default:
    case TabPaths.AccountManager:
      defaultValue = 0;
      break;
    case TabPaths.PasswordManager:
      defaultValue = 1;
      break;
  }
  const [value, setValue] = React.useState(defaultValue);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };
  return (
    <Hidden mdDown>
      <AppBar position='static' color='default'>
        <Tabs
          value={value}
          onChange={handleChange}
          variant='scrollable'
          scrollButtons='on'
        >
          <Tab label={props.translate.accountManager} icon={<AccountBalanceIcon />} component={Link} to={TabPaths.AccountManager} />
          <Tab label={props.translate.passwordManager} icon={<VpnKeyIcon />} component={Link} to={TabPaths.PasswordManager} />
        </Tabs>
      </AppBar>
    </Hidden>
  );
}

const mapStateToProps = store => ({
  translate: {
    passwordManager: store.lang.translation.home.passwordManager,
    accountManager: store.lang.translation.home.accountManager,
  },
  dark: store.theme
});
export default withRouter(connect(mapStateToProps)(NavTabs));