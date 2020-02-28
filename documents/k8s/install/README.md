[TOC]

## k8s集群安装
##### 安装 [Chocolatey](https://chocolatey.org/install)

* 运行 ``` Get-ExecutionPolicy ``` ，如果返回 ``` Restricted```  ,则执行 ``` Get-ExecutionPolicy``` 或 ```Set-ExecutionPolicy Bypass -Scope Process```
* 然后执行安装 ```Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))```

##### 安装 [terminals](https://chocolatey.org/packages/terminals)

* ``` choco install terminals ```

##### 使用虚拟机安装centos，并使用ssh连接
```
# 开启网卡
# vi /etc/sysconfig/network-scripts/ifcfg-ens33
# 重启网络
# service network restart
# 修改 ONBOOT=yes
# yum install net-tools

ssh root@192.168.103.217  # Master
# root123
ssh root@192.168.103.218  # Node
# root123

# 禁用防火墙
systemctl stop firewalld
systemctl disable firewalld

# 禁用SELinux
# 永久禁用 重启生效
vi /etc/selinux/config
reboot
# 修改 SELINUX=disabled
# 临时禁用
# setenforce 0

# 禁用swap
swapoff -a
# 修改 /etc/fstab
# 在行首家#，注释 /dev/mapper/centos-swap swap
# 保存 swapon -s
# 验证
free -m

# 设置iptables
echo 1 > /proc/sys/net/bridge/bridge-nf-call-iptables
echo 1 > /proc/sys/net/bridge/bridge-nf-call-ip6tables
```

##### 安装docker
官方地址：[https://docs.docker.com/install/linux/docker-ce/centos/](https://docs.docker.com/install/linux/docker-ce/centos/)
阿里云安装docker：
```
# 下载 wget 工具
yum install wget
# 更换阿里云的源
sudo vi /etc/resolv.conf
nameserver 223.5.5.5
nameserver 223.6.6.6
sudo cp /etc/yum.repos.d/CentOS-Base.repo /etc/yum.repos.d/CentOS-Base.repo.bak
sudo wget -O /etc/yum.repos.d/CentOS-Base.repo http://mirrors.aliyun.com/repo/Centos-7.repo
sudo wget -P /etc/yum.repos.d/ http://mirrors.aliyun.com/repo/epel-7.repo
sudo yum clean all
sudo yum makecache
sudo yum update
```
自动安装
```
curl -fsSL https://get.docker.com | bash -s docker --mirror Aliyun
```
手动安装
```
# step 1: 安装必要的一些系统工具
sudo yum install -y yum-utils device-mapper-persistent-data lvm2
# Step 2: 添加软件源信息
sudo yum-config-manager --add-repo http://mirrors.aliyun.com/docker-ce/linux/centos/docker-ce.repo
# Step 3: 更新并安装 Docker-CE
sudo yum makecache fast
sudo yum -y install docker-ce
# Step 4: 开启Docker服务
sudo service docker start
systemctl enable docker

注意：其他注意事项在下面的注释中
# 官方软件源默认启用了最新的软件，您可以通过编辑软件源的方式获取各个版本的软件包。例如官方并没有将测试版本的软件源置为可用，你可以通过以下方式开启。同理可以开启各种测试版本等。
# vim /etc/yum.repos.d/docker-ce.repo
#   将 [docker-ce-test] 下方的 enabled=0 修改为 enabled=1
#
# 安装指定版本的Docker-CE:
# Step 1: 查找Docker-CE的版本:
# yum list docker-ce.x86_64 --showduplicates | sort -r
#   Loading mirror speeds from cached hostfile
#   Loaded plugins: branch, fastestmirror, langpacks
#   docker-ce.x86_64            17.03.1.ce-1.el7.centos            docker-ce-stable
#   docker-ce.x86_64            17.03.1.ce-1.el7.centos            @docker-ce-stable
#   docker-ce.x86_64            17.03.0.ce-1.el7.centos            docker-ce-stable
#   Available Packages
# Step2 : 安装指定版本的Docker-CE: (VERSION 例如上面的 17.03.0.ce.1-1.el7.centos)
# sudo yum -y install docker-ce-[VERSION]
# 注意：在某些版本之后，docker-ce安装出现了其他依赖包，如果安装失败的话请关注错误信息。例如 docker-ce 17.03 之后，需要先安装 docker-ce-selinux。
# yum list docker-ce-selinux- --showduplicates | sort -r
# sudo yum -y install docker-ce-selinux-[VERSION]

# 通过经典网络、VPC网络内网安装时，用以下命令替换Step 2中的命令
# 经典网络：
# sudo yum-config-manager --add-repo http://mirrors.aliyuncs.com/docker-ce/linux/centos/docker-ce.repo
# VPC网络：
# sudo yum-config-manager --add-repo http://mirrors.could.aliyuncs.com/docker-ce/linux/centos/docker-ce.repo
```

配置镜像加速
```
sudo mkdir -p /etc/docker
sudo tee /etc/docker/daemon.json <<-'EOF'
{
  "registry-mirrors": ["https://uwxsp1y1.mirror.aliyuncs.com"]
}
EOF
sudo systemctl daemon-reload
sudo systemctl restart docker
```

##### 安装 k8s
```
cat <<EOF > /etc/yum.repos.d/kubernetes.repo
[kubernetes]
name=Kubernetes
baseurl=https://mirrors.aliyun.com/kubernetes/yum/repos/kubernetes-el7-x86_64/
enabled=1
gpgcheck=1
repo_gpgcheck=1
gpgkey=https://mirrors.aliyun.com/kubernetes/yum/doc/yum-key.gpg https://mirrors.aliyun.com/kubernetes/yum/doc/rpm-package-key.gpg
EOF
# 禁用 SELinux
setenforce 0
# 安装
yum install -y kubelet kubeadm kubectl
systemctl enable kubelet && systemctl start kubelet
# 测试
kubectl
kubeadm
```

##### 初始化 master 节点
完整的官方文档可以参考：
https://kubernetes.io/docs/setup/independent/create-cluster-kubeadm/
https://kubernetes.io/docs/reference/setup-tools/kubeadm/kubeadm-init/
```
kubeadm init \
    --apiserver-advertise-address=192.168.103.217 \
    --image-repository registry.aliyuncs.com/google_containers \
    --kubernetes-version v1.17.0 \
    --pod-network-cidr=10.244.0.0/16
```
成功
```
Your Kubernetes control-plane has initialized successfully!

To start using your cluster, you need to run the following as a regular user:

  mkdir -p $HOME/.kube
  sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config
  sudo chown $(id -u):$(id -g) $HOME/.kube/config

You should now deploy a pod network to the cluster.
Run "kubectl apply -f [podnetwork].yaml" with one of the options listed at:
  https://kubernetes.io/docs/concepts/cluster-administration/addons/

Then you can join any number of worker nodes by running the following on each as root:

kubeadm join 192.168.103.217:6443 --token mr9xwc.fumyuepc3bfuz2n5 \
    --discovery-token-ca-cert-hash sha256:0341251ba038989d3900298f9fe86d1d67d156d5fbb28512f2a30d5b023c308e
```
执行创建文件
```
mkdir -p $HOME/.kube
sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config
sudo chown $(id -u):$(id -g) $HOME/.kube/config
```
验证
```
kubectl get nodes
NAME                    STATUS     ROLES    AGE    VERSION
localhost.localdomain   NotReady   master   5m5s   v1.17.3
```

