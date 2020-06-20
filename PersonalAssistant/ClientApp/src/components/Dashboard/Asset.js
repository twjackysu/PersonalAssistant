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

export default class Asset extends Component {
    static displayName = Asset.name;

    constructor(props) {
        super(props);
        this.state = {
            latestAccountBalance: [],
            latestStockValue: []
        };
    }
    async setData() {
        let lastestAccountBalanceTask = populateData('/api/GetLatestAccountBalance');
        let latestStockValueTask = populateData('/api/GetLatestStockValue');
        let lastestAccountBalance = await lastestAccountBalanceTask;
        let latestStockValue = await latestStockValueTask;
        this.setState({
            latestAccountBalance: lastestAccountBalance,
            latestStockValue: latestStockValue
        });
    }
    componentDidMount() {
        this.setData();
    }
    render() {
        let { latestAccountBalance, latestStockValue } = this.state;
        let total = 0;
        latestAccountBalance.forEach(element => {
            total += element.Balance;
        });
        latestStockValue.forEach(element => {
            total += element.Balance;
        });
        return (
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <StyledTableRow>
                            <StyledTableCell align='center'>Asset</StyledTableCell>
                            <StyledTableCell align='center'>Balance</StyledTableCell>
                        </StyledTableRow>
                    </TableHead>
                    <TableBody>
                        {latestAccountBalance.map((item, index) =>
                            <StyledTableRow key={index}>
                                <StyledTableCell align='center'>{item.Name}</StyledTableCell>
                                <StyledTableCell align='center'>{item.Balance}</StyledTableCell>
                            </StyledTableRow>)}
                        {latestStockValue.map((item, index) =>
                            <StyledTableRow key={index}>
                                <StyledTableCell align='center'>{item.Name}</StyledTableCell>
                                <StyledTableCell align='center'>{item.Date ? `${item.Balance} ${item.Date}` : item.Balance}</StyledTableCell>
                            </StyledTableRow>)}
                        <StyledTableRow>
                            <StyledTableCell align='center'>Total</StyledTableCell>
                            <StyledTableCell align='center'>{total}</StyledTableCell>
                        </StyledTableRow>
                    </TableBody>
                </Table>
            </TableContainer>
        );
    }
}
