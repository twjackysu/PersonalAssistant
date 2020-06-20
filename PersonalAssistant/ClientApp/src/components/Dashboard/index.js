    import React from 'react';
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
  return (
    <Grid container spacing={2}>
      <Grid item xs={12} sm={6}>
        <Typography variant="h5" gutterBottom>
          目前資產
        </Typography>
        <Asset />
      </Grid>
      <Grid item xs={12} sm={6}>
        <Typography variant="h5" gutterBottom>
          前一個月消費
        </Typography>
        <OneMonthCost />
      </Grid>
      <Grid item xs={12} sm={6}>
        <Typography variant="h5" gutterBottom>
          每日平均消費
        </Typography>
        <AvgEveryDayCost />
      </Grid>
      <Grid item xs={12} sm={6}>
        <History />
      </Grid>
    </Grid>
  );

}
export default Dashboard;