import { withStyles } from '@material-ui/core/styles';
import TableCell from '@material-ui/core/TableCell';

const BodyCell = withStyles({
    root: {
        padding: '4px',
        fontSize: '1rem'
    },
})(TableCell);

export default BodyCell;