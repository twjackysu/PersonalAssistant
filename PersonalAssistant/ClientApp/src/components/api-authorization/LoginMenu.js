import React, { Component, Fragment } from 'react';
import { connect } from "react-redux";
import { Link } from 'react-router-dom';
import Button from '@material-ui/core/Button';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import authService from './AuthorizeService';
import { ApplicationPaths } from './ApiAuthorizationConstants';

class LoginMenu extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isAuthenticated: false,
            userName: null
        };
    }

    componentDidMount() {
        this._subscription = authService.subscribe(() => this.populateState());
        this.populateState();
    }

    componentWillUnmount() {
        authService.unsubscribe(this._subscription);
    }

    async populateState() {
        const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()])
        this.setState({
            isAuthenticated,
            userName: user && user.name
        });
    }

    render() {
        const { isAuthenticated, userName } = this.state;
        if (!isAuthenticated) {
            const registerPath = `${ApplicationPaths.Register}`;
            const loginPath = `${ApplicationPaths.Login}`;
            if (this.props.drawer) {
                return this.anonymousViewDrawer(registerPath, loginPath);
            } else {
                return this.anonymousView(registerPath, loginPath);
            }
        } else {
            const profilePath = `${ApplicationPaths.Profile}`;
            const logoutPath = { pathname: `${ApplicationPaths.LogOut}`, state: { local: true } };
            if (this.props.drawer) {
                return this.authenticatedViewDrawer(userName, profilePath, logoutPath);
            } else {
                return this.authenticatedView(userName, profilePath, logoutPath);
            }
        }
    }

    authenticatedView(userName, profilePath, logoutPath) {
        return (<Fragment>
            <Button color="inherit" component={Link} to={profilePath}>{this.props.hello} {userName}</Button>
            <Button color="inherit" component={Link} to={logoutPath}>{this.props.logout}</Button>
        </Fragment>);
    }

    authenticatedViewDrawer(userName, profilePath, logoutPath) {
        return (<Fragment>
            <ListItem button component={Link} to={profilePath}>
                <ListItemText primary={this.props.hello + ' ' + userName} />
            </ListItem>
            <ListItem button component={Link} to={logoutPath}>
                <ListItemText primary={this.props.logout} />
            </ListItem>
        </Fragment>);
    }

    anonymousView(registerPath, loginPath) {
        return (<Fragment>
            <Button color="inherit" component={Link} to={registerPath}>{this.props.register}</Button>
            <Button color="inherit" component={Link} to={loginPath}>{this.props.login}</Button>
        </Fragment>);
    }
    anonymousViewDrawer(registerPath, loginPath) {
        return (<Fragment>
            <ListItem button component={Link} to={registerPath}>
                <ListItemText primary={this.props.register} />
            </ListItem>
            <ListItem button component={Link} to={loginPath}>
                <ListItemText primary={this.props.login} />
            </ListItem>
        </Fragment>);
    }
}

const mapStateToProps = store => ({
    hello: store.lang.translation.menu.hello,
    register: store.lang.translation.menu.register,
    login: store.lang.translation.menu.login,
    logout: store.lang.translation.menu.logout
});
export default connect(mapStateToProps)(LoginMenu);