import React from 'react';

interface QueryHilightedProps {
    text: string;
    hilight: string;
}

export function QueryHilighted(props: QueryHilightedProps) {
    const {text, hilight} = props;
    const parts = text.split(new RegExp(`(${hilight})`, 'gi'));
    
    return <span>{parts.map((part, i) => 
        part.toLowerCase() === hilight.toLowerCase() 
            ? <em key={i}>{part}</em> 
            : <span key={i}>{part}</span>
    )}</span>;
}