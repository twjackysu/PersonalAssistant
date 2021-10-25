import React, { Component } from 'react';
import { connect } from "react-redux";
import { CssBaseline } from '@material-ui/core';
import { createMuiTheme, MuiThemeProvider } from '@material-ui/core/styles';
import NavMenu from './NavMenu';

const darkTheme = createMuiTheme({
  palette: {
    type: 'dark'
  },
  typography: {
    fontFamily: ['"cwTeXYen"', 'sans-serif'].join(','),
    fontSize: 20
  }
});
const lightTheme = createMuiTheme({
  palette: {
    type: 'light'
  },
  typography: {
    fontFamily: ['"cwTeXYen"', 'sans-serif'].join(','),
    fontSize: 20
  }
});

class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <MuiThemeProvider theme={this.props.dark ? darkTheme : lightTheme}>
        <CssBaseline />
        <NavMenu />
        {this.props.children}
      </MuiThemeProvider>
    );
  }
}


const mapStateToProps = store => ({ dark: store.theme });
export default connect(mapStateToProps)(Layout);