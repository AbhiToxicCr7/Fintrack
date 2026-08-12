import {
  Bar,
  BarChart,
  CartesianGrid,
  XAxis,
  YAxis,
  Tooltip,
  Rectangle,
} from "recharts";

export default function ExpenseIncomeBarChart({data}){
    return(
        <BarChart width={600} height={300} data={data}>
            <Bar
                dataKey = "income"
                fill="#f4f44e"
                activeBar={<Rectangle fill="gold" stroke="purple" />}
            />
            <Bar
                dataKey="expense"
                fill="#b04a0b"
                activeBar={<Rectangle fill="pink" stroke="purple" />}
            />
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip cursor={{ fill: "transparent" }} />
        </BarChart>
    )
}