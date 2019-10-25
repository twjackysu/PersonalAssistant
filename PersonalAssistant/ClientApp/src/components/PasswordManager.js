import React from 'react';
import { connect } from "react-redux";
import { Grid } from '@material-ui/core';
import PasswordGenerate from './PasswordGenerate';

const PasswordManager = props => {
  return (
    <Grid container spacing={10}>
      <Grid item xs={12} md={6}>
        <PasswordGenerate />
      </Grid>
    </Grid>
  );
}

const mapStateToProps = store => ({
  translate: {
  }
});
export default connect(mapStateToProps)(PasswordManager);