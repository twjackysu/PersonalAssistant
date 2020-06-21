import React from 'react';
import { connect } from 'react-redux';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import Asset from './Asset';
import OneMonthCost from './OneMonthCost';
import AvgEveryDayCost from './AvgEveryDayCost';
import History from './History';


const Dashboard = (props) => {
  // this.state = { 
  //   avgEveryDayCost: {},
  //   oneMonthIncome: 0,
  //   avgEveryDayIncome: 0
  // };
  const { translate } = props;
  return (
    <Grid container spacing={2}>
      <Grid item xs={12} sm={6}>
        <Typography variant="h5" gutterBottom>
          {translate.currentAsset}
        </Typography>
        <Asset />
      </Grid>
      <Grid item xs={12} sm={6}>
        <Typography variant="h5" gutterBottom>
          {translate.expenditure30Days}
        </Typography>
        <OneMonthCost />
      </Grid>
      <Grid item xs={12} sm={6}>
        <Typography variant="h5" gutterBottom>
          {translate.avgDailyExpenditure}
        </Typography>
        <AvgEveryDayCost />
      </Grid>
      <Grid item xs={12} sm={6}>
        <History />
      </Grid>
    </Grid>
  );

}

const mapStateToProps = store => ({
  translate: {
    currentAsset: store.lang.translation.accountManager.CurrentAsset,
    expenditure30Days: store.lang.translation.accountManager.Expenditure30Days,
    avgDailyExpenditure: store.lang.translation.accountManager.AvgDailyExpenditure,
  }
});
export default connect(mapStateToProps)(Dashboard);