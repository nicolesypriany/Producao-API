import { Flex } from "@mantine/core";
import { Link } from "react-router";
import { routes } from "./routes";

const SidebarRoutes = () => {
  const styles = {
    textDecoration: "none",
    color: "black",
  };

  return (
    <Flex gap="sm" direction="column" align="flex-start">
      {routes.map(({ id, label, path }) => (
        <Link key={id} to={path} style={styles}>
          {label}
        </Link>
      ))}
    </Flex>
  );
};

export default SidebarRoutes;
