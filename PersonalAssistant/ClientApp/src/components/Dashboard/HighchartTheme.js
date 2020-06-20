import red from '@material-ui/core/colors/red';
import blue from '@material-ui/core/colors/blue';
import green from '@material-ui/core/colors/green';
import yellow from '@material-ui/core/colors/yellow';
import purple from '@material-ui/core/colors/purple';
import indigo from '@material-ui/core/colors/indigo';
import teal from '@material-ui/core/colors/teal';
import orange from '@material-ui/core/colors/orange';
import brown from '@material-ui/core/colors/brown';
import pink from '@material-ui/core/colors/pink';
import deepPurple from '@material-ui/core/colors/deepPurple';
import lightBlue from '@material-ui/core/colors/lightBlue';
import lightGreen from '@material-ui/core/colors/lightGreen';
import amber from '@material-ui/core/colors/amber';
import grey from '@material-ui/core/colors/grey';
import cyan from '@material-ui/core/colors/cyan';
import lime from '@material-ui/core/colors/lime';
import deepOrange from '@material-ui/core/colors/deepOrange';
import blueGrey from '@material-ui/core/colors/blueGrey';

const HighchartTheme = {
    colors: [red[400], blue[400], green[400], yellow[400], purple[400], indigo[400], teal[400], orange[400], brown[400], pink[400], deepPurple[400], 
             lightBlue[400], lightGreen[400], amber[400], grey[400], cyan[400], lime[400], deepOrange[400], blueGrey[400]],
    chart: {
        backgroundColor: '#272c34',
        borderRadius: 20
    },
    title: {
        style: {
            color: '#fff',
        }
    },
    legend: {
        itemStyle: {
            color: '#aaa'
        },
        itemHoverStyle:{
            color: '#fff'
        }   
    },
    yAxis: {
    		labels: {
        		style: {
            		color: '#aaa'
            }
        },
    		title: {
        		style: {
            		color: '#aaa'
            }
        }
    },
    xAxis: {
    		labels: {
        		style: {
            		color: '#aaa'
            }
        }
    }
};

export default HighchartTheme;