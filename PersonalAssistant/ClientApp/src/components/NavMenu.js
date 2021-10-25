import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from "react-redux";
import { AppBar, Toolbar, Typography, IconButton, Tooltip, Hidden, Grid, Select, MenuItem } from '@material-ui/core';
import MenuIcon from '@material-ui/icons/Menu';
import TranslateIcon from '@material-ui/icons/Translate';
import Brightness2Icon from '@material-ui/icons/Brightness2';
import WbSunnyIcon from '@material-ui/icons/WbSunny';

import { change_language, change_theme, switch_menu_list } from "../redux/actions";
import enus from '../translation/en-us';
import zhtw from '../translation/zh-tw';
import LoginMenu from './api-authorization/LoginMenu';
import NavDrawer from './NavDrawer';

const NavMenu = props => {
  let themeIcon = props.dark ? <Brightness2Icon onChange={() => props.change_theme()} /> : <WbSunnyIcon onChange={() => props.change_theme()} />;
  return (
    <AppBar position='static' color='default'>
      <Toolbar>
        <Grid container direction='row' justify='space-between' alignItems='center'>
          <Grid item>
            <Grid container direction='row' alignItems='center'>
              <Grid item>
                <IconButton edge='start' color='inherit' aria-label='menu' onClick={(e) => props.switch_menu_list(true)}>
                  <MenuIcon />
                </IconButton>
              </Grid>
              <Grid item>
                <Typography variant='h5' color='inherit' align='center' component={Link} to='/' >
                  PersonalAssistant
                </Typography>
              </Grid>
            </Grid>
          </Grid>
          <Hidden mdDown>
            <Grid item>
              <Grid container direction='row' alignItems='center'>
                <Grid item>
                  <Select value={props.translate.code} onChange={(e) => props.change_language(e.target.value)}>
                    <MenuItem value={enus.code}><TranslateIcon fontSize='small' />{enus.language}</MenuItem>
                    <MenuItem value={zhtw.code}><TranslateIcon fontSize='small' />{zhtw.language}</MenuItem>
                  </Select>
                </Grid>
                <Grid item>
                  <Tooltip title='Switch theme' aria-label="Switch theme">
                    <IconButton onClick={() => props.change_theme()}>
                      {themeIcon}
                    </IconButton>
                  </Tooltip>
                </Grid>
                <Grid item>
                  <LoginMenu />
                </Grid>
              </Grid>
            </Grid>
          </Hidden>
        </Grid>
        <NavDrawer />
      </Toolbar>
    </AppBar>
  );
};


const mapStateToProps = store => ({
  translate: {
    code: store.lang.translation.code
  },
  dark: store.theme
});
export default connect(
  mapStateToProps,
  { change_language, change_theme, switch_menu_list }
)(NavMenu);