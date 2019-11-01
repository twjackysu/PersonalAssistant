import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from "react-redux";
import { Collapse, Drawer, List, Divider, ListItem, ListItemIcon, ListItemText } from '@material-ui/core';
import VpnKeyIcon from '@material-ui/icons/VpnKey';
import TranslateIcon from '@material-ui/icons/Translate';
import Brightness2Icon from '@material-ui/icons/Brightness2';
import AccountBalanceIcon from '@material-ui/icons/AccountBalance';
import ExpandLess from '@material-ui/icons/ExpandLess';
import ExpandMore from '@material-ui/icons/ExpandMore';
import WbSunnyIcon from '@material-ui/icons/WbSunny';
import { withStyles } from '@material-ui/core/styles';

import { change_language, change_theme, switch_menu_list } from "../redux/actions";
import enus from '../translation/en-us';
import zhtw from '../translation/zh-tw';
import LoginMenu from './api-authorization/LoginMenu';
import { TabPaths } from '../helper/RouterConstants';

const styles = theme => ({
  nested: {
    paddingLeft: theme.spacing(4),
  },
});

const NavDrawer = props => {
  const [openLanguage, setOpen] = React.useState(false);

  const handleClick = () => {
    setOpen(!openLanguage);
  };
  let themeIcon = props.dark ? <Brightness2Icon onChange={() => props.change_theme()} /> : <WbSunnyIcon onChange={() => props.change_theme()} />;
  return (
    <Drawer open={props.showMenu} onClose={(e) => props.switch_menu_list(false)}>
      <List>
        <ListItem button component={Link} to={TabPaths.AccountManager}>
          <ListItemIcon>
            <AccountBalanceIcon />
          </ListItemIcon>
          <ListItemText primary={props.translate.accountManager} />
        </ListItem>
        <ListItem button component={Link} to={TabPaths.PasswordManager}>
          <ListItemIcon>
            <VpnKeyIcon />
          </ListItemIcon>
          <ListItemText primary={props.translate.passwordManager} />
        </ListItem>
      </List>
      <Divider />
      <List>
        <ListItem button onClick={handleClick}>
          <ListItemIcon>
            <TranslateIcon />
          </ListItemIcon>
          <ListItemText primary={props.translate.changeLanguage} />
          {openLanguage ? <ExpandLess /> : <ExpandMore />}
        </ListItem>
        <Collapse in={openLanguage} timeout='auto' unmountOnExit>
          <List disablePadding>
            <ListItem className={props.classes.nested} onClick={() => props.change_language(enus.code)} button>
              <ListItemText primary={enus.language} />
            </ListItem>
            <ListItem className={props.classes.nested} onClick={() => props.change_language(zhtw.code)} button>
              <ListItemText primary={zhtw.language} />
            </ListItem>
          </List>
        </Collapse>
        <ListItem button onClick={() => props.change_theme()}>
          <ListItemIcon>{themeIcon}</ListItemIcon>
          <ListItemText primary={props.translate.switchTheme} />
        </ListItem>
        <LoginMenu drawer />
      </List>
    </Drawer>
  );
};


const mapStateToProps = store => ({
  translate: {
    code: store.lang.translation.code,
    passwordManager: store.lang.translation.home.passwordManager,
    accountManager: store.lang.translation.home.accountManager,
    switchTheme: store.lang.translation.menu.switchTheme,
    changeLanguage: store.lang.translation.menu.changeLanguage,
  },
  dark: store.theme,
  showMenu: store.showMenu
});
export default connect(
  mapStateToProps,
  { change_language, change_theme, switch_menu_list }
)(withStyles(styles)(NavDrawer));