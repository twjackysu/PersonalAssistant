import React from 'react';
import { connect } from "react-redux";
import { Link } from 'react-router-dom';
import { Button, Grid } from '@material-ui/core';
import { createMuiTheme, MuiThemeProvider } from '@material-ui/core/styles';
import VpnKeyIcon from '@material-ui/icons/VpnKey';
import AccountBalanceIcon from '@material-ui/icons/AccountBalance';
//import { change_language } from "../redux/actions";
const overrideMuiButton = {
  root: {
    fontSize: '2.4rem'
  }
};
const darkTheme = createMuiTheme({
  palette: {
    type: 'dark'
  },
  overrides: {
    MuiButton: overrideMuiButton
  }
});
const lightTheme = createMuiTheme({
  palette: {
    type: 'light'
  },
  overrides: {
    MuiButton: overrideMuiButton
  }
});
const Home = props => {
  return (
    <MuiThemeProvider theme={props.dark ? darkTheme : lightTheme}>
      <Grid container spacing={10}>
        <Grid item xs={12} md={6}>
          <Button
            size='large'
            variant='outlined'
            component={Link} to='/passwordManager'
            startIcon={<VpnKeyIcon fontSize='large' />}
            fullWidth>
            {props.translate.passwordManager}
          </Button>
        </Grid>
        <Grid item xs={12} md={6}>
          <Button
            size='large'
            variant='outlined'
            component={Link} to='/accountManager'
            startIcon={<AccountBalanceIcon fontSize='large' />}
            fullWidth>
             {props.translate.accountManager}
          </Button>
        </Grid>
      </Grid>
    </MuiThemeProvider>
  );
}

const mapStateToProps = store => ({
  translate: {
    passwordManager: store.lang.translation.home.passwordManager,
    accountManager: store.lang.translation.home.accountManager,
  },
  dark: store.theme
});
export default connect(mapStateToProps)(Home);