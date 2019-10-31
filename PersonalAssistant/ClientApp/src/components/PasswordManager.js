import React from 'react';
import { connect } from "react-redux";
import { Grid, Typography, Container } from '@material-ui/core';
import GeneratePassword from './GeneratePassword';

const PasswordManager = props => {
  return (
    <Container>
      <Grid container spacing={10}>
        <Grid item xs={12} md={6}>
          <GeneratePassword />
        </Grid>
        <Grid item xs={12} md={6}>
          <Typography variant="h5" gutterBottom>
            {props.translate.explanation}
          </Typography>
        </Grid>
      </Grid>
    </Container>
  );
}

const mapStateToProps = store => ({
  translate: {
    explanation: store.lang.translation.passwordManager.explanation,
  }
});
export default connect(mapStateToProps)(PasswordManager);