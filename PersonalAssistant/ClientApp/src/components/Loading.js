import React from 'react';
import Grid from '@material-ui/core/Grid';
import CircularProgress from '@material-ui/core/CircularProgress';

const Loading = props => {
    return (
    <Grid container justify='center' alignItems='center'>
        <Grid item>
            <CircularProgress />
        </Grid>
    </Grid>
    );
}

export default Loading;