import React from 'react';
import { connect } from "react-redux";
import { Grid } from '@material-ui/core';
import GeneratePassword from './GeneratePassword';

const PasswordManager = props => {
  return (
    <Grid container spacing={10}>
      <Grid item xs={12} md={6}>
        <GeneratePassword />
      </Grid>
    </Grid>
  );
}

const mapStateToProps = store => ({
  translate: {
  }
});
export default connect(mapStateToProps)(PasswordManager);