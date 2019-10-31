import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from "react-redux";
import { AppBar, Toolbar, Typography, IconButton, Tooltip, Hidden, Collapse, Grid, Select, MenuItem
    , Drawer, List, Divider, ListItem, ListItemIcon, ListItemText } from '@material-ui/core';
import MenuIcon from '@material-ui/icons/Menu';
import VpnKeyIcon from '@material-ui/icons/VpnKey';
import TranslateIcon from '@material-ui/icons/Translate';
import Brightness2Icon from '@material-ui/icons/Brightness2';
import ExpandLess from '@material-ui/icons/ExpandLess';
import ExpandMore from '@material-ui/icons/ExpandMore';
import WbSunnyIcon from '@material-ui/icons/WbSunny';
import { withStyles } from '@material-ui/core/styles';

import { change_language, change_theme, switch_menu_list } from "../redux/actions";
import enus from '../translation/en-us';
import zhtw from '../translation/zh-tw';
import LoginMenu from './api-authorization/LoginMenu';

const styles = theme => ({
    nested: {
        paddingLeft: theme.spacing(4),
    },
});

const NavMenu = props => {
    const [open, setOpen] = React.useState(false);

    const handleClick = () => {
        setOpen(!open);
    };
    let themeIcon = props.dark ? <Brightness2Icon onChange={() => props.change_theme()} /> : <WbSunnyIcon onChange={() => props.change_theme()} />;
    return (
            <AppBar position='static' color='default'>
                <Toolbar>
                    <Grid container direction='row' justify='space-between' alignItems='center'>
                        <Grid item>
                            <Grid container direction='row' alignItems='center'>
                                <Hidden mdUp>
                                    <Grid item>
                                        <IconButton edge='start' color='inherit' aria-label='menu' onClick={(e) => props.switch_menu_list(true)}>
                                            <MenuIcon />
                                        </IconButton>
                                    </Grid>
                                </Hidden>
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
                    <Drawer open={props.showMenu} onClose={(e) => props.switch_menu_list(false)}>
                        <List>
                            <ListItem button component={Link} to='/passwordManager'>
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
                                {open ? <ExpandLess /> : <ExpandMore />}
                            </ListItem>
                            <Collapse in={open} timeout="auto" unmountOnExit>
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
                </Toolbar>
            </AppBar>
    );
};


const mapStateToProps = store => ({
    translate: {
        code: store.lang.translation.code,
        passwordManager: store.lang.translation.home.passwordManager,
        switchTheme: store.lang.translation.menu.switchTheme,
        changeLanguage: store.lang.translation.menu.changeLanguage,
    },
    dark: store.theme,
    showMenu: store.showMenu
});
export default connect(
    mapStateToProps,
    { change_language, change_theme, switch_menu_list }
)(withStyles(styles)(NavMenu));