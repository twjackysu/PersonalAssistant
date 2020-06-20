import React from 'react';
import { connect } from "react-redux";
import Typography from '@material-ui/core/Typography';
import Container from '@material-ui/core/Container';

const Home = props => {
  return (
      <Container>
        <Typography variant="h4" gutterBottom>
          {props.translate.announcement}
        </Typography>
      </Container>
  );
}

const mapStateToProps = store => ({
  translate: {
    announcement: store.lang.translation.home.announcement,
  }
});
export default connect(mapStateToProps)(Home);