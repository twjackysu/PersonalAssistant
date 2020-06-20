import React, { Component } from 'react';
import { withStyles } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';

import { populateData } from '../../helper/CommonFunc';
const StyledTableCell = withStyles((theme) => ({
    head: {
        backgroundColor: theme.palette.common.black,
        color: theme.palette.common.white,
    },
    body: {
        fontSize: '1.2rem',
    },
}))(TableCell);

const StyledTableRow = withStyles((theme) => ({
    root: {
        '&:nth-of-type(odd)': {
            backgroundColor: theme.palette.action.hover,
        },
    },
}))(TableRow);

export default class AvgEveryDayCost extends Component {
    static displayName = AvgEveryDayCost.name;

    constructor(props) {
        super(props);
        this.state = {
            avgEveryDayCost: {}
        };
    }
    async setData(){
        let avgEveryDayCost = await populateData('/api/GetAvgEveryDayCost');
        this.setState({
            avgEveryDayCost: avgEveryDayCost
        });
    }
    componentDidMount() {
        this.setData();
    }
    render() {
        let { avgEveryDayCost } = this.state;
        let bodyRows = [];
        let count = 0;
        for(let key in avgEveryDayCost){
            bodyRows.push(<StyledTableRow key={count}>
                <StyledTableCell align='center'>{key}</StyledTableCell>
                <StyledTableCell align='center'>{avgEveryDayCost[key]}</StyledTableCell>
            </StyledTableRow>);
            count++;
        }
        return (
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <StyledTableRow>
                            <StyledTableCell align='center'>Type</StyledTableCell>
                            <StyledTableCell align='center'>Cost</StyledTableCell>
                        </StyledTableRow>
                    </TableHead>
                    <TableBody>
                        {bodyRows}
                    </TableBody>
                </Table>
            </TableContainer>
        );
    }
}
