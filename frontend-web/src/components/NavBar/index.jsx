import { pathRoutes } from "@/routers";
import { Anchor, Avatar, Code, Group } from "@mantine/core";
import { useState } from "react";
import {
  TbBellRinging,
  TbDatabaseImport,
  TbFingerprint,
  TbKey,
  TbLogout,
  TbReceipt2,
  TbSettings,
} from "react-icons/tb";
import { Outlet, useNavigate } from "react-router";
import classes from "./navbar.module.css";

const data = [
  { link: pathRoutes.formas, label: "Formas", icon: TbFingerprint },
  { link: pathRoutes.machines, label: "Máquinas", icon: TbBellRinging },
  { link: pathRoutes.rawMaterial, label: "Matéria-prima", icon: TbReceipt2 },
  { link: "", label: "Produções", icon: TbKey },
  { link: "", label: "Produto", icon: TbDatabaseImport },
  { link: "", label: "Usuários", icon: TbSettings },
];

const user = {
  name: "Nicolas Junkes",
  avatar: "https://avatars.githubusercontent.com/u/68435848?v=4",
};

export default function PublicLayout() {
  const [active, setActive] = useState("Formas");
  const navigate = useNavigate();

  const links = data.map((item) => {
    return (
      <Anchor
        className={classes.link}
        data-active={item.label === active || undefined}
        href={item.link}
        key={item.label}
        onClick={(event) => {
          event.preventDefault();
          setActive(item.label);
          navigate(item.link);
        }}
      >
        <item.icon className={classes.linkIcon} />
        <span>{item.label}</span>
      </Anchor>
    );
  });

  return (
    <>
      <nav className={classes.navbar}>
        <div className={classes.navbarMain}>
          <Group className={classes.header} justify="space-between">
            <Anchor href={pathRoutes.home}>Produção</Anchor>
            <Code fw={700}>v3.1.2</Code>
          </Group>
          {links}
        </div>

        <div className={classes.footer}>
          <Anchor
            href="#"
            className={classes.link}
            onClick={(event) => event.preventDefault()}
          >
            <Avatar
              // src={user.avatar}
              name={user.name}
              className={classes.linkIcon}
            />
            <span>Nicolas Junkes</span>
          </Anchor>

          <Anchor
            href="#"
            className={classes.link}
            onClick={(event) => event.preventDefault()}
          >
            <TbLogout className={classes.linkIcon} />
            <span>Logout</span>
          </Anchor>
        </div>
      </nav>
      <div>
        <Outlet />
      </div>
    </>
  );
}
