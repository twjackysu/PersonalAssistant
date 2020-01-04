import React, { Fragment } from 'react';
import CircularProgress from '@material-ui/core/CircularProgress';
import Backdrop from '@material-ui/core/Backdrop';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    marginTop: {
        marginTop: theme.spacing(5),
    },
    backdrop: {
        zIndex: theme.zIndex.drawer + 1,
        color: '#fff',
    },
}));

export default function Loading(props) {
    const classes = useStyles();
    return (
        <Backdrop
            className={classes.backdrop}
            open={true}
        >
            <CircularProgress color="inherit" />
        </Backdrop>
    );
}