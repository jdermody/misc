import React from 'react';

export interface RangeProps {
    amount: number;
    max: number;
}

export function Range(props: RangeProps) {
    const {amount, max} = props;
    return <div className="range">
        <div className="amount" style={{width: 100 - (amount * 100 / max) + '%'}}/>
        <div className="value">{amount}</div>
    </div>
}