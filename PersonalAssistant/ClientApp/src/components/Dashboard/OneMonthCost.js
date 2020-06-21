import React, { Component } from 'react';
import { connect } from 'react-redux';
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

class OneMonthCost extends Component {
    static displayName = OneMonthCost.name;

    constructor(props) {
        super(props);
        this.state = {
            oneMonthCost: {}
        };
    }
    async setData() {
        let oneMonthCost = await populateData('/api/Get1MonthCost');
        this.setState({
            oneMonthCost: oneMonthCost
        });
    }
    componentDidMount() {
        this.setData();
    }
    render() {
        const { oneMonthCost } = this.state;
        const { translate } = this.props;
        let bodyRows = [];
        let count = 0;
        let total = 0;
        for (let key in oneMonthCost) {
            bodyRows.push(<StyledTableRow key={count}>
                <StyledTableCell align='center'>{key === 'Fee'? translate.fees: key}</StyledTableCell>
                <StyledTableCell align='center'>{oneMonthCost[key]}</StyledTableCell>
            </StyledTableRow>);
            count++;
            total+=oneMonthCost[key];
        }
        return (
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <StyledTableRow>
                            <StyledTableCell align='center'>{translate.type}</StyledTableCell>
                            <StyledTableCell align='center'>{translate.cost}</StyledTableCell>
                        </StyledTableRow>
                    </TableHead>
                    <TableBody>
                        {bodyRows}
                        <StyledTableRow>
                            <StyledTableCell align='center'>{translate.total}</StyledTableCell>
                            <StyledTableCell align='center'>{total}</StyledTableCell>
                        </StyledTableRow>
                    </TableBody>
                </Table>
            </TableContainer>
        );
    }
}

const mapStateToProps = store => ({
    translate: {
        type: store.lang.translation.accountManager.Type,
        cost: store.lang.translation.accountManager.Cost,
        total: store.lang.translation.accountManager.Total,
        fees:  store.lang.translation.accountManager.Fees,
    }
});
export default connect(mapStateToProps)(OneMonthCost);