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

export default class OneMonthCost extends Component {
    static displayName = OneMonthCost.name;

    constructor(props) {
        super(props);
        this.state = {
            oneMonthCost: {}
        };
    }
    async setData(){
        let oneMonthCost = await populateData('/api/Get1MonthCost');
        this.setState({
            oneMonthCost: oneMonthCost
        });
    }
    componentDidMount() {
        this.setData();
    }
    render() {
        let { oneMonthCost } = this.state;
        let bodyRows = [];
        let count = 0;
        for(let key in oneMonthCost){
            bodyRows.push(<StyledTableRow key={count}>
                <StyledTableCell align='center'>{key}</StyledTableCell>
                <StyledTableCell align='center'>{oneMonthCost[key]}</StyledTableCell>
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
