import React from 'react';
import { connect } from "react-redux";
import Typography from '@material-ui/core/Typography';
import Container from '@material-ui/core/Container';

import AddOrEditForm from './AddOrEditForm';

const Home = props => {
  const [values, setValues] = React.useState(['text','bbb','2020-08-20 11:30:50', 5000]);
  let fieldsInfo = [{
    headText: '',
    fieldName: '',
  }];
  console.log(values);
  return (
      <Container>
        <Typography variant="h4" gutterBottom>
          {props.translate.announcement}
        </Typography>
        <AddOrEditForm 
        title='Income'
        values={values}
        setValues={setValues}
        fieldsInfo={[
          {type:'text', required: true, label: 'Amount'},
          {type:'select', required: false, label: 'Amount', selectOptions: [{value: 'aaa', label: 'AAA'},{value: 'bbb', label: 'BBB'}]},
          {type:'dateTime', required: true, label: 'Amount'},
          {type:'number', required: true, label: 'Amount'},
        ]} />
      </Container>
  );
}

const mapStateToProps = store => ({
  translate: {
    announcement: store.lang.translation.home.announcement,
  }
});
export default connect(mapStateToProps)(Home);