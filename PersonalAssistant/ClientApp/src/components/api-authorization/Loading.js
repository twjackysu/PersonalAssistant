import React, { Fragment } from 'react';
import LinearProgress from '@material-ui/core/LinearProgress';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    marginTop: {
        marginTop: theme.spacing(5),
    },
}));

export default function Loading(props) {
    const classes = useStyles();
    return (
        <Fragment>
            <Typography variant='h5' className={classes.marginTop}>{props.text}</Typography>
            <LinearProgress className={classes.marginTop} />
        </Fragment>
    );
}