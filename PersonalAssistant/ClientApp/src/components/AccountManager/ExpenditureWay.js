import React, { Component } from 'react';
import { connect } from 'react-redux';
import { withStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import AddBoxIcon from '@material-ui/icons/AddBox';
import IconButton from '@material-ui/core/IconButton';
import Link from '@material-ui/core/Link';

import oidc from '../oidc';
import { HttpMethod } from '../../helper/GeneralConstants';
import { WebApi } from '../../helper/RouterConstants';
import Loading from '../Loading';
import SharedTable from '../SharedTable';
import { inputType } from '../../helper/GeneralConstants';
import { TabPaths } from '../../helper/RouterConstants';
import EditDialog from './EditDialog';


const styles = {
};

class ExpenditureWay extends Component {
    static displayName = Expenditure.name;
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            apiResult: [],
        };
    }
    componentDidMount() {
        oidc(HttpMethod.Get, WebApi.ExpenditureWays).then(json => {
            this.setState({ apiResult: json, loading: false });
        });
    }

    render() {
        const { apiResult, loading } = this.state;
        if (loading) {
            return <Loading />
        }
        
        const { controller, translate, classes } = this.props;
        
        const columnsInfo = [
            {
                headText: 'Expenditure Way',
                align: 'center',
                fieldName: 'WayName'
            }
        ];
        return (
            <Grid container direction='row' justify='flex-end' alignItems='flex-start'>
                <Grid item xs='auto'>
                    <IconButton component={Link} to={TabPaths.AccountManager.Index}>
                        <AddBoxIcon />
                    </IconButton>
                </Grid>
                <Grid item xs={12}>
                    <SharedTable data={apiResult} columnsInfo={columnsInfo} />
                </Grid>
                <Grid item xs={12}>
                    <EditDialog oidc={this.oidc} />
                </Grid>
            </Grid>
        );
    }
}

const mapStateToProps = store => ({
    translate: {
    }
});
export default connect(mapStateToProps)(withStyles(styles)(ExpenditureWay));