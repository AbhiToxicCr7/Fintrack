import {Cell, Pie, PieChart, Tooltip} from "recharts";

 const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#ff4242'];

 const renderCustomizedLabel = ({
  cx,
  cy,
  midAngle,
  innerRadius,
  outerRadius,
  percent,
}) => {
  const RADIAN = Math.PI / 180;
  const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
  const x = cx + radius * Math.cos(-midAngle * RADIAN);
  const y = cy + radius * Math.sin(-midAngle * RADIAN);

  return (
    <text
      x={x}
      y={y}
      fill="white"
      textAnchor={x > cx ? "start" : "end"}
      dominantBaseline="central"
    >
      {`${(percent * 100).toFixed(0)}%`}
    </text>
  );
};

 export default function ExpensesChart({data}){
    return (
        <PieChart width={700} height={360}>
            <Pie data={data} dataKey='value' nameKey='name' label={renderCustomizedLabel} labelLine={false}>
                {data.map((entry, index)=>(
                    <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]}/>
                ))}
            </Pie>
            <Tooltip/>
        </PieChart>
    )
 }